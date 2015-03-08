using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaCube.Core.Repositories
{
    public class InMemoryAddressSpaceManagerRepository : IAddressSpaceManagerRepository
    {
        private string _dimensionCode;

        private List<int> _allocatedBits;
        private Queue<int> _releasedBits;

        public InMemoryAddressSpaceManagerRepository()
        {
            LocalIndex = -1;
            _dimensionCode = null;
            _allocatedBits = null;
            _releasedBits = null;
        }

        /// <summary>
        /// Current Index
        /// </summary>
        public int LocalIndex { get; set; }

        public IEnumerable<int> AllocatedBits
        {
            get { return _allocatedBits; }
        }

        public IEnumerable<int> ReleasedBits
        {
            get { return _releasedBits; }
        }

        public int AllocatedBitsCount
        {
            get { return _allocatedBits.Count; }
        }

        public void AllocateBit(int bit)
        {
            _allocatedBits.Add(bit);
        }

        public void ReleaseBit(int bit)
        {
            _releasedBits.Enqueue(bit);
        }

        public int? GetReleasedBit()
        {
            if(_releasedBits.Count > 0)
            {
                return _releasedBits.Dequeue();
            }
            else
            {
                return null;
            }
        }

        public void SetAllocatedBits(IEnumerable<int> list)
        {
            _allocatedBits = new List<int>(list);
        }

        public void SetReleasedBits(IEnumerable<int> list)
        {
            _releasedBits = new Queue<int>(list);
        }
    }
}
