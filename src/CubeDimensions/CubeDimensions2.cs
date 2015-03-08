using DynaCube.Core;
using DynaCube.Core.BitSpace;
using DynaCube.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeDimensions
{   
    public class CubeDimensions2
    {        
        private readonly HashSet<long> state;
        private readonly Dictionary<string, Dimension> dimensions;
        private readonly DimensionManager dimensionManaager;

        public CubeDimensions2()
        {
            this.dimensionManaager = new DimensionManager(new BitSpaceManager(64), new BitSpaceManager(64), new NopPersistantStorage());
            this.state = new HashSet<long>();
            this.dimensions = new Dictionary<string, Dimension>();
        }

        //private Dimension GetDimension(string dimensionCode)
        //{
        //    Dimension result;

        //    if (!dimensions.TryGetValue(dimensionCode, out result))
        //    {
        //        result = new Dimension(dimensionCode, bitspace.GetNewAddressSpace(dimensionCode));
        //        dimensions.Add(dimensionCode, result);    
        //    }                

        //    return result;
        //}

        //private int GetAddress(DimensionValue value)
        //{
        //    var dimenson = GetDimension(value.DimensionCode);

        //    DimensionValueAddressed result;

        //    if (!dimenson.DimensionValues.TryGetValue(value.DimensionValueCode, out result))
        //    {
        //        var address = (int)dimenson.AddressSpace.GetAddress();
        //        result = new DimensionValueAddressed(value.DimensionCode, value.DimensionValueCode, address);
        //        dimenson.DimensionValues.Add(value.DimensionValueCode, result);
        //    }          

        //    return result.Address;
        //}
        
        public void AddPoint(IEnumerable<DimensionValue> point)
        {
            long address = GetAddress(point);


            //Console.WriteLine("Add: {0}-{1}", String.Join(",", point.Select(p => p.DimensionValueCode)), address);
            state.Add(address);
        }

        private long GetAddress(IEnumerable<DimensionValue> point)
        {
            //int address = point.Sum(p => GetAddress(p));
            long address = 0;

            foreach (var p in point)
            {
                var dimension = dimensionManaager.AddOrGetDimension(p.DimensionCode);
                var value = dimension.AddOrGetValue(p.DimensionValueCode, p.DimensionValueCode);
                address += value.Index;
            }
            return address;
        }

        public IEnumerable<CubeDimensionValue> Enumerate(IEnumerable<DimensionValue> point, string dimensionCode)
        {
            var dimension = dimensionManaager.AddOrGetDimension(dimensionCode);

            var address = GetAddress(point);
            foreach(var dimensionValue in dimension.Values)
            {
                if (state.Contains(address + dimensionValue.Index))
                {
                    var temp = point.Union(new[] { new DimensionValue(dimensionValue.DimensionCode, dimensionValue.DimensionValueCode) });
                    Console.WriteLine("Get: {0}-{1}", String.Join(",", temp.Select(p => p.DimensionValueCode)), address + dimensionValue.Index);
                    yield return dimensionValue;
                }
            }
        }
    }
}
