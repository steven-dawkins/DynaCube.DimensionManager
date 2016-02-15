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
    public class CubeDimensionTracker
    {        
        private readonly HashSet<long> state;        
        private readonly DimensionManager dimensionManager;

        public CubeDimensionTracker()
        {
            this.dimensionManager = new DimensionManager(new BitSpaceManager(64), new BitSpaceManager(64), new NopPersistantStorage());
            this.state = new HashSet<long>();            
        }        
        
        public void AddPoint(IEnumerable<DimensionValue> point)
        {            
            var values = point.Select(p => dimensionManager.AddOrGetDimension(p.DimensionCode).AddOrGetValue(p.DimensionValueCode, p.DimensionValueCode));

            AddValuePartial(0, new Stack<CubeDimensionValue>(values), 0, true);
        }

        private void AddValuePartial(long index, Stack<CubeDimensionValue> right, int dimensions, bool includeSuperAggregate)
        {
            if (right.Count == 0)
            {
                if (state.Count % 100000 == 0)
                {
                    Console.WriteLine(state.Count);
                }

                if (dimensions == 0 && !includeSuperAggregate)
                {
                    return;
                }

                state.Add(index);               
            }
            else
            {
                var head = right.Pop();

                AddValuePartial(index + head.Index, right, dimensions + 1, includeSuperAggregate);
                AddValuePartial(index, right, dimensions, includeSuperAggregate);

                right.Push(head);
            }
        }

        private long GetAddress(IEnumerable<DimensionValue> point)
        {            
            long address = 0;

            foreach (var p in point)
            {
                var dimension = dimensionManager.AddOrGetDimension(p.DimensionCode);
                var value = dimension.AddOrGetValue(p.DimensionValueCode, p.DimensionValueCode);
                address += value.Index;
            }
            return address;
        }

        public IEnumerable<CubeDimensionValue> Enumerate(IEnumerable<DimensionValue> point, string dimensionCode)
        {
            var dimension = dimensionManager.AddOrGetDimension(dimensionCode);

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
