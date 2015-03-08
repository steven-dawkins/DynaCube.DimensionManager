using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaCube.Core.Serialization.Models
{
    public class BitSpaceManagerState
    {
        public List<int> AllocatedBits { get; set; }

        public List<int> AvailableBits { get; set; }
    }
}
