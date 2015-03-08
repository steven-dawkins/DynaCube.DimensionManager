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

    public class DimensionValue
    {
        public readonly string DimensionCode;
        public readonly string DimensionValueCode;

        public DimensionValue(string dimensionCode, string dimensionValueCode)
        {
            this.DimensionCode = dimensionCode;
            this.DimensionValueCode = dimensionValueCode;
        }

        public override string ToString()
        {
            return DimensionValueCode;
        }
    }

    public class CubeDimensions2
    {        
        private readonly HashSet<long> state;        
        private readonly DimensionManager dimensionManaager;

        public CubeDimensions2()
        {
            this.dimensionManaager = new DimensionManager(new BitSpaceManager(64), new BitSpaceManager(64), new NopPersistantStorage());
            this.state = new HashSet<long>();            
        }
        
        
        public void AddPoint(IEnumerable<DimensionValue> point)
        {
            long address = GetAddress(point);

            //Console.WriteLine("Add: {0}-{1}", String.Join(",", point.Select(p => p.DimensionValueCode)), address);
            state.Add(address);
        }

        private long GetAddress(IEnumerable<DimensionValue> point)
        {            
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
