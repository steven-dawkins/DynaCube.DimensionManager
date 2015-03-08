using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaCube.Core.Repositories
{
    public class InMemoryBitSpaceManagerRepository : IBitSpaceManagerRepository
    {
        private Queue<int> _availableBits;
        private List<int> _allocatedBits;

        public InMemoryBitSpaceManagerRepository()
        {
            _availableBits = null;
            _allocatedBits = null;
        }

        public int DequeueAvailableBit()
        {
            return _availableBits.Dequeue();
        }

        public void EnqueueAvailableBit(int bit)
        {
            _availableBits.Enqueue(bit);
        }

        public void AddAllocatedBit(int bit)
        {
            _allocatedBits.Add(bit);
        }
        public void RemoveAllocatedBit(int bit)
        {
            _allocatedBits.Remove(bit);
        }        

        public void SetAvailableBits(IEnumerable<int> availableBits)
        {
            this._availableBits = new Queue<int>(availableBits);
        }

        public void SetAllocatedBits(IEnumerable<int> allocatedBits)
        {
            this._allocatedBits = new List<int>(allocatedBits);
        }

        public bool AvailableBitsContains(int bit)
        {
            return _availableBits.Contains(bit);
        }

        public bool AllocatedBitsContains(int bit)
        {
            return _allocatedBits.Contains(bit);
        }

        public int AllocatedBitsCount
        {
            get { return _allocatedBits.Count; }
        }

        public IEnumerable<int> AvailableBits
        {
            get { return _availableBits; }
        }

        public IEnumerable<int> AllocatedBits
        {
            get { return _allocatedBits; }
        }
    }
}
