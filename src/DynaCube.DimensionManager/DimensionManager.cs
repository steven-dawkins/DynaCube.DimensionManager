using DynaCube.Core.BitSpace;
using DynaCube.Core.Repositories;
using DynaCube.Core.Serialization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaCube.Core
{
    public class DimensionManager
    {
        private readonly Dictionary<string, CubeDimension> _dimensions;
        private readonly BitSpaceManager _bitspaceManager;
        private readonly BitSpaceManager _offsetBitManager;
        private readonly IPersistantStorage _persistantStorage;


        /// <summary>
        /// Restore dimension manager from saved state
        /// </summary>
        /// <param name="state"></param>
        public DimensionManager(DimensionManagerState state)
        {
            this._dimensions = new Dictionary<string, CubeDimension>();
            this._bitspaceManager = new BitSpaceManager(state.BitSpace);
            this._offsetBitManager = new BitSpaceManager(state.OffsetBitSpace);
            this._persistantStorage = new NopPersistantStorage(); // TODO: should restore persistant storage

            foreach (var dimension in state.Dimensions)
            {
                if (dimension.IsContiguous)
                {
                    var addressStateManager = new AddressSpaceManager(_offsetBitManager, dimension.Address);

                    this.AddOrGetDimension(dimension.DimensionCode, addressStateManager, dimension.IsContiguous);
                }
                else
                {
                    var addressStateManager = new AddressSpaceManager(_bitspaceManager, dimension.Address);

                    this.AddOrGetDimension(dimension.DimensionCode, addressStateManager, dimension.IsContiguous);
                }                
            }

            foreach (var dv in state.DimensionValues)
            {
                this.AddOrGetDimensionValue(dv.DimensionCode, dv.DimensionValueCode, dv.DimensionValueName, dv.Index);
            }
        }


        /// <summary>
        /// create new dimension manager object
        /// </summary>
        /// <param name="bitspaceManager">bitspace to allocate dimensions</param>
        /// <param name="persistantStorage">persistant storage provider</param>
        public DimensionManager(BitSpaceManager bitspaceManager, BitSpaceManager offsetBitspaceManager, IPersistantStorage persistantStorage)
        {
            this._dimensions = new Dictionary<string, CubeDimension>();
            this._bitspaceManager = bitspaceManager;
            this._offsetBitManager = offsetBitspaceManager;
            this._persistantStorage = persistantStorage;

            foreach (var dimension in _persistantStorage.Dimensions)
            {
                var bitstate = _bitspaceManager.GetNewAddressSpace(dimension.DimensionCode);
                AddOrGetDimension(dimension.DimensionCode, bitstate, dimension.IsContiguous);
            }
        }

        /// <summary>
        /// Retrieve dimension by code
        /// </summary>
        /// <param name="dimensionCode">dimension code</param>
        /// <returns></returns>
        public CubeDimension this[string dimensionCode]
        {
            get
            {
                if (_dimensions.ContainsKey(dimensionCode))
                {
                    return _dimensions[dimensionCode];
                }
                else
                {
                    return null;
                }
            }
        }

        public IEnumerable<CubeDimension> Dimensions
        {
            get { return _dimensions.Values; }
        }


        public int DimensionCount
        {
            get { return _dimensions.Count; }
        }

        
        /// <summary>
        /// Add new dimension or retrieve if already exists
        /// </summary>
        /// <param name="dimensionCode"></param>
        /// <returns></returns>
        public CubeDimension AddOrGetDimension(string dimensionCode, bool? contiguous = null)
        {
            contiguous = contiguous ?? dimensionCode == "Year";

            if (!_dimensions.ContainsKey(dimensionCode))
            {                
                if (contiguous == true)
                {
                    var bitstate = _offsetBitManager.GetNewAddressSpace(dimensionCode);

                    var newDimension = AddOrGetDimension(dimensionCode, bitstate, true);
                    return newDimension;
                }
                else
                {
                    var bitstate = _bitspaceManager.GetNewAddressSpace(dimensionCode);

                    var newDimension = AddOrGetDimension(dimensionCode, bitstate, false);
                    return newDimension;
                }
            }
            else
            {
                return _dimensions[dimensionCode];
            }
        }

        /// <summary>
        /// Restores dimension from saved state
        /// </summary>
        /// <param name="dimensionCode"></param>
        /// <param name="bitstate"></param>
        /// <returns></returns>
        private CubeDimension AddOrGetDimension(string dimensionCode, AddressSpaceManager bitstate, bool isContiguous)
        {
            // TODO: this should be create only and throw exception if it already exists
            if (!_dimensions.ContainsKey(dimensionCode))
            {
                var c = new CubeDimension(dimensionCode, bitstate, _persistantStorage, isContiguous);

                _dimensions.Add(dimensionCode, c);
                _persistantStorage.UpsertDimension(c.GetState());
                return c;
            }
            else
            {
                return _dimensions[dimensionCode];
            }
        }

        private CubeDimensionValue AddOrGetDimensionValue(string dimensionCode, string dimensionValueCode, string dimensionValueName, long index)
        {
            var dimension = this[dimensionCode];
            return dimension.AddOrGetValue(dimensionValueCode, dimensionValueName, index);
        }
        

        public DimensionManagerState GetState()
        {
            return new DimensionManagerState
            {
                BitSpace = _bitspaceManager.GetState(),
                OffsetBitSpace = _offsetBitManager.GetState(),
                Dimensions = from dimension in Dimensions
                             select dimension.GetState(),
                DimensionValues =
                            from value in Dimensions
                                                .SelectMany(values => values.Values)
                                                .Cast<CubeDimensionValue>()
                            select value.GetState(),
            };
        }
    }
}
