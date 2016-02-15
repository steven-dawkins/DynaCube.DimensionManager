using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaCube.Core.Serialization.Models
{
    public class DimensionState
    {
        public string DimensionCode { get; private set; }

        public AddressSpaceManagerState Address { get; private set; }

        public bool IsContiguous { get; private set; }

        public DimensionState(string dimensionCode, AddressSpaceManagerState address, bool isContiguous)
        {
            DimensionCode = dimensionCode;
            Address = address;
            IsContiguous = isContiguous;
        }
    }
}
