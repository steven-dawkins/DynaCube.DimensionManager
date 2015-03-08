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
    /// <summary>
    /// Cube dimension, contains AddressSpaceManager to allocate indexes
    /// </summary>
    public class CubeDimension : GenericDimension<CubeDimensionValue>
    {
        private readonly AddressSpaceManager _address;
        private readonly IPersistantStorage _storage;
        private readonly bool _isContiguous;
                
        /// <summary>
        /// Restore cube dimension from serialized state
        /// </summary>
        /// <param name="dimensionCode">Dimension code</param>
        /// <param name="address">Dimension address space</param>
        public CubeDimension(string dimensionCode, AddressSpaceManager address, IPersistantStorage storage, bool isContiguous) :
            base(dimensionCode)
        {
            this._address = address;
            this._storage = storage;
            this._isContiguous = isContiguous;           
        }        

        /// <summary>
        /// Add Dimension value
        /// </summary>
        /// <param name="dimensionValueCode">Dimension value code</param>
        /// <param name="dimensionValueName">Dimension valude description</param>
        public CubeDimensionValue AddOrGetValue(string dimensionValueCode, string dimensionValueName)
        {
            if (!_values.ContainsKey(dimensionValueCode))
            {
                var index = _address.GetAddress();
                var newDimension = AddOrGetValue(dimensionValueCode, dimensionValueName, index);
                
                return newDimension;
            }
            else
            {
                return _values[dimensionValueCode];
            }
        }

        /// <summary>
        /// Add Dimension value with explicit index
        /// </summary>
        /// <param name="dimensionValueCode">Dimension value code</param>
        /// <param name="dimensionValueName">Dimension valude description</param>
        public CubeDimensionValue AddOrGetValue(string dimensionValueCode, string dimensionValueName, long index)
        {
            if (!_values.ContainsKey(dimensionValueCode))
            {
                var value = new CubeDimensionValue(this.DimensionCode, dimensionValueCode, dimensionValueName, index, this._isContiguous);

                AddValue(value);
                _storage.UpsertDimensionValue(value.GetState());
                return value;
            }
            else
            {
                return _values[dimensionValueCode];
            }
        }

        public DimensionState GetState()
        {
            return new DimensionState(DimensionCode, _address.GetState(), _isContiguous);
        }

        public override string ToString()
        {
            return String.Format("{0} - bits {1}", DimensionCode, _address.NumberOfBitsInUse);
        }

        protected override CubeDimensionValue Create(string dimensionCode, string dimensionValueCode, string dimensionValueName)
        {
            return new CubeDimensionValue(dimensionCode, dimensionValueCode, dimensionValueName, 0, this._isContiguous);
        }
    }   
}
