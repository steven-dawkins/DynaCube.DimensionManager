using DynaCube.Core.BitSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeDimensions
{
    public class Dimension
    {
        public readonly string DimensionCode;
        public readonly AddressSpaceManager AddressSpace;
        public readonly Dictionary<string, DimensionValueAddressed> DimensionValues;

        public Dimension(string dimensionCode, AddressSpaceManager addressSpace)
        {
            this.DimensionCode = dimensionCode;
            this.AddressSpace = addressSpace;
            this.DimensionValues = new Dictionary<string, DimensionValueAddressed>();
        }
    }

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

    public class DimensionValueAddressed : DimensionValue
    {
        public readonly int Address;

        public DimensionValueAddressed(string dimensionCode, string dimensionValueCode, int address)
            : base(dimensionCode, dimensionValueCode)
        {
            this.Address = address;
        }
    }

    public class CubeDimensions
    {
        private readonly BitSpaceManager bitspace;
        private readonly HashSet<int> state;
        private readonly Dictionary<string, Dimension> dimensions;

        public CubeDimensions()
        {
            this.bitspace = new BitSpaceManager(64);
            this.state = new HashSet<int>();
            this.dimensions = new Dictionary<string, Dimension>();
        }

        private Dimension GetDimension(string dimensionCode)
        {
            Dimension result;

            if (!dimensions.TryGetValue(dimensionCode, out result))
            {
                result = new Dimension(dimensionCode, bitspace.GetNewAddressSpace(dimensionCode));
                dimensions.Add(dimensionCode, result);    
            }                

            return result;
        }

        private int GetAddress(DimensionValue value)
        {
            var dimenson = GetDimension(value.DimensionCode);

            DimensionValueAddressed result;

            if (!dimenson.DimensionValues.TryGetValue(value.DimensionValueCode, out result))
            {
                var address = (int)dimenson.AddressSpace.GetAddress();
                result = new DimensionValueAddressed(value.DimensionCode, value.DimensionValueCode, address);
                dimenson.DimensionValues.Add(value.DimensionValueCode, result);
            }          

            return result.Address;
        }
        
        public void AddPoint(IEnumerable<DimensionValue> point)
        {            
            //int address = point.Sum(p => GetAddress(p));
            int address = 0;

            foreach (var p in point)
            {
                address += GetAddress(p);
            }


            Console.WriteLine("Add: {0}-{1}", String.Join(",", point.Select(p => p.DimensionValueCode)), address);
            state.Add(address);
        }

        public IEnumerable<DimensionValue> Enumerate(IEnumerable<DimensionValue> point, string dimensionCode)
        {
            var dimension = GetDimension(dimensionCode);

            var address = point.Sum(p => GetAddress(p));
            foreach(var dimensionValue in dimension.DimensionValues.Values)
            {
                if (state.Contains(address + dimensionValue.Address))
                {
                    var temp = point.Union(new[] { dimensionValue });
                    Console.WriteLine("Get: {0}-{1}", String.Join(",", temp.Select(p => p.DimensionValueCode)), address + dimensionValue.Address);
                    yield return dimensionValue;
                }
            }
        }
    }
}
