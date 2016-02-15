using DynaCube.Core.Serialization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaCube.Core.Repositories
{
#warning remove
    public interface IPersistantStorage
    {
        IEnumerable<DimensionState> Dimensions { get; }

        void UpsertDimension(DimensionState dimensionState);

        void UpsertDimensionValue(DimensionValueState dimensionValueState);
    }
}
