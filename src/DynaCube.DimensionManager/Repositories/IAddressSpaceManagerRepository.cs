using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaCube.Core.Repositories
{
    public interface IAddressSpaceManagerRepository
    {
        int LocalIndex { get; set; }
        IEnumerable<int> AllocatedBits { get; }
        IEnumerable<int> ReleasedBits { get; }
        int AllocatedBitsCount { get; }

        void AllocateBit(int bit);
        void ReleaseBit(int bit);               

        int? GetReleasedBit();

        void SetAllocatedBits(IEnumerable<int> list);
        void SetReleasedBits(IEnumerable<int> list);
    }
}
