using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaCube.Core
{
    public interface IDimension
    {
        string DimensionCode { get; }
        IEnumerable<DimensionValue> Values { get; }
        // TODO: why is this a long?
        long ValueCount { get; }
        DimensionValue this[string dimensionValueCode] { get; }
        DimensionValue this[int dimensionValueIndex] { get; }

        DimensionValue TotalDimension { get; }
    }
}
