using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaCube.Core.Repositories
{
    public interface IBitSpaceManagerRepository
    {
        bool AvailableBitsContains(int bit);
        bool AllocatedBitsContains(int bit);
        
        void SetAvailableBits(IEnumerable<int> list);
        void SetAllocatedBits(IEnumerable<int> list);

        int AllocatedBitsCount { get; }

        IEnumerable<int> AvailableBits { get; }
        IEnumerable<int> AllocatedBits { get; }        

        int DequeueAvailableBit();
        void RemoveAllocatedBit(int bit);
        
        void EnqueueAvailableBit(int bit);
        void AddAllocatedBit(int bit);
    }
}
