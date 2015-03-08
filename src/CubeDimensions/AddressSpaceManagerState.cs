using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaCube.Core.Serialization.Models
{
    public class AddressSpaceManagerState
    {
        public int Index { get; set; }

        public List<int> ReleasedPool { get; set; }

        public List<int> AllocatedBits { get; set; }        
    }
}
