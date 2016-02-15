using DynaCube.Core.Serialization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynaCube.Core
{
    public class DimensionManagerState
    {
        public BitSpaceManagerState BitSpace { get; set; }
        public IEnumerable<DimensionState> Dimensions { get; set; }

        public IEnumerable<DimensionValueState> DimensionValues { get; set; }


        public BitSpaceManagerState OffsetBitSpace { get; set; }
    }
}
