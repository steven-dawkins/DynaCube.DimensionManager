using DynaCube.Core.Serialization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaCube.Core
{
    public class CubeDimensionValue : DimensionValue
    {        
        public long Index { get; private set; }
        public bool IsContiguous { get; private set; }

        public CubeDimensionValue(string dimensionCode, string dimensionValueCode, string dimensionValueName, long index, bool isContiguous)
            : base(dimensionCode, dimensionValueCode, dimensionValueName)
        {
            Index = index;
            IsContiguous = isContiguous;
        }

        internal DimensionValueState GetState()
        {
            return new DimensionValueState
                            {
                                DimensionCode = this.DimensionCode,
                                DimensionValueCode = this.DimensionValueCode,
                                DimensionValueName = this.DimensionValueName,
                                Index = this.Index
                            };
        }        
    }
}
