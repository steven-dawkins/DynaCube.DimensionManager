using DynaCube.Core.Repositories;
using DynaCube.Core.Serialization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaCube.Core.BitSpace
{
    /// <summary>
    /// Manage address space for a cube dimension
    /// </summary>
    public class AddressSpaceManager
    {        
        private readonly BitSpaceManager _bitPool;
        private readonly IAddressSpaceManagerRepository _repository;

        public AddressSpaceManager(BitSpaceManager bitManager)
            : this(bitManager, new InMemoryAddressSpaceManagerRepository())
        {
        }

        public AddressSpaceManager(BitSpaceManager bitManager, AddressSpaceManagerState addressSpaceManagerState)
            : this(bitManager, addressSpaceManagerState, new InMemoryAddressSpaceManagerRepository())
        {
        }

        /// <summary>
        /// Initialise a new AddressSpaceManager instance
        /// </summary>
        /// <param name="bitManager">global pool of bits</param>
        public AddressSpaceManager(BitSpaceManager bitManager, IAddressSpaceManagerRepository repository)
        {
            this._bitPool = bitManager;
            this._repository = repository;
            this._repository.SetAllocatedBits(new int[] {});
            this._repository.SetReleasedBits(new int[] { });            
            this._repository.LocalIndex = 1;
        }

        /// <summary>
        /// Restore a new AddressSpaceManager instance from saved state
        /// </summary>
        /// <param name="bitManager">global pool of bits</param>
        public AddressSpaceManager(BitSpaceManager bitManager, AddressSpaceManagerState addressSpaceManagerState, IAddressSpaceManagerRepository repository)
        {
            this._repository = repository;
            this._bitPool = bitManager;
            this._repository.SetAllocatedBits(addressSpaceManagerState.AllocatedBits);
            this._repository.SetReleasedBits(addressSpaceManagerState.ReleasedPool);
            this._repository.LocalIndex = addressSpaceManagerState.Index;
        }

        /// <summary>
        /// Number of bit in use in this address space
        /// </summary>
        public int NumberOfBitsInUse
        {
            get
            {
                return this._repository.AllocatedBitsCount;
            }
        }
              
        /// <summary>
        /// Release an address
        /// </summary>
        /// <param name="value">value to release</param>
        public void ReleaseValue(int value)
        {
            _repository.ReleaseBit(value);            
        }

        /// <summary>
        /// Request next available address
        /// </summary>
        /// <returns>new address</returns>
        public long GetAddress()
        {
            // check for released bit which can be re-used
            var releasedBit = _repository.GetReleasedBit();
            if (releasedBit.HasValue)
            {
                return releasedBit.Value;
            }
                           

            // expand address space if needed
            while (_repository.LocalIndex > GetRange())
            {
                ExpandAddressSpace();
            }

            var namespaceAddress = 0L;
            var c = 1;
            foreach(var bit in _repository.AllocatedBits)
            {
                if ((_repository.LocalIndex & c) > 0)
                {
                    namespaceAddress += (long)Math.Pow(2, bit);
                }

                c = c << 1;
            }

            // increment for next access
            _repository.LocalIndex++;

            return namespaceAddress;
        }


        /// <summary>
        /// Get size of currently allocated address space
        /// </summary>
        /// <returns></returns>
        private int GetRange()
        {
            if (_repository.AllocatedBitsCount <= 0)
            {
                return 0;
            }
            else
            {
                return (int)Math.Pow(2, _repository.AllocatedBitsCount) - 1;
            }
        }  

        /// <summary>
        /// Grab a bit from bitmanager to expand address space
        /// </summary>
        private void ExpandAddressSpace()
        {
            _repository.AllocateBit(_bitPool.GetBit());
        }

        internal AddressSpaceManagerState GetState()
        {
            return new AddressSpaceManagerState
            {                
                AllocatedBits = _repository.AllocatedBits.ToList(),
                ReleasedPool = _repository.ReleasedBits.ToList(),
                Index = _repository.LocalIndex,
            };
        }
    }
}
