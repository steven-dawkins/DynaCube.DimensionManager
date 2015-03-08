using DynaCube.Core.Serialization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaCube.Core.Repositories
{
    public class NopPersistantStorage : IPersistantStorage
    {
        public void UpsertDimension(DimensionState dimensionState)
        {
            
        }


        public void UpsertDimensionValue(DimensionValueState dimensionValueState)
        {           
        }

        public IEnumerable<Serialization.Models.DimensionState> Dimensions
        {
            get { return new DimensionState[] { }; }
        }
    }
}
