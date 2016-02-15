using DynaCube.Core.Factories;
using DynaCube.Core.Repositories;
using DynaCube.Core.Serialization.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaCube.Core.BitSpace
{    
    /// <summary>
    /// manages bit space
    /// </summary>
    public class BitSpaceManager
    {
        private readonly IBitSpaceManagerRepository _repository;
        private readonly AddressSpaceManagerRepositoryFactory _addressSpaceManagerRepositoryFactory;
        
        public BitSpaceManager(int numberOfBits, bool randomize = true)
            : this(numberOfBits, new InMemoryBitSpaceManagerRepository(), new InMemoryAddressSpaceManagerFactory(), randomize)
        {
        }

        /// <summary>
        /// Initialize bit space manager
        /// </summary>
        /// <param name="numberOfBits">number of bits to manage</param>
        public BitSpaceManager(int numberOfBits, IBitSpaceManagerRepository repository, AddressSpaceManagerRepositoryFactory addressSpaceManagerFactory, bool randomize = true)
        {
            Trace.WriteLine(string.Format("Initialising BitSpaceManager with {0} bits", numberOfBits));
            
            if (numberOfBits > 64)
            {
                throw new ArgumentException("bit range exceeded: " + numberOfBits, "numberOfBits");
            }

            if (numberOfBits <= 0)
            {
                throw new ArgumentException("invalid number of bits: " + numberOfBits, "numberOfBits");
            }

            this._repository = repository;
            this._addressSpaceManagerRepositoryFactory = addressSpaceManagerFactory;
            
            var rand = new Random(42);
            var bits = Enumerable.Range(0, numberOfBits);

            if (randomize)
            {
                // Randomize bits to improve hashing
                bits = bits.OrderBy(i => rand.Next());
            }

            this._repository.SetAvailableBits(bits);
            this._repository.SetAllocatedBits(new int[] {});
        }

        public BitSpaceManager(BitSpaceManagerState bitSpaceManagerState)
            : this(bitSpaceManagerState, new InMemoryBitSpaceManagerRepository(), new InMemoryAddressSpaceManagerFactory())
        {
        }

        /// <summary>
        /// Restore BitSpaceManager from serialized state
        /// </summary>
        /// <param name="bitSpaceManagerState"></param>
        public BitSpaceManager(BitSpaceManagerState bitSpaceManagerState, IBitSpaceManagerRepository repository, AddressSpaceManagerRepositoryFactory addressSpaceManagerFactory)
        {
            this._repository = repository;
            this._addressSpaceManagerRepositoryFactory = addressSpaceManagerFactory;

            this._repository.SetAvailableBits(bitSpaceManagerState.AvailableBits);
            this._repository.SetAllocatedBits(bitSpaceManagerState.AllocatedBits);            
        }

        public int AllocatedBits
        {
            get
            {
                return _repository.AllocatedBitsCount;
            }
        }

        /// <summary>
        /// Request bit
        /// </summary>
        /// <returns></returns>
        public int GetBit()
        {
            var bit = _repository.DequeueAvailableBit();
            _repository.AddAllocatedBit(bit);
            return bit;            
        }

        /// <summary>
        /// Release bit
        /// </summary>
        /// <param name="bit"></param>
        public void ReleaseBit(int bit)
        {
            if (_repository.AvailableBitsContains(bit))
            {
                throw new ArgumentException("cannot release bit which is already available: " + bit, "bit");
            }
            if (!_repository.AllocatedBitsContains(bit))
            {
                throw new ArgumentException("Attempt to release unallocated bit");
            }
            
            _repository.RemoveAllocatedBit(bit);
            _repository.EnqueueAvailableBit(bit);
        }

        internal BitSpaceManagerState GetState()
        {
            return new BitSpaceManagerState
            {
                AvailableBits = this._repository.AvailableBits.ToList(),
                AllocatedBits = this._repository.AllocatedBits.ToList()
            };
        }

        internal AddressSpaceManager GetNewAddressSpace(string dimensionCode)
        {
            var repository = this._addressSpaceManagerRepositoryFactory.GetAddressSpaceManagerRepository(dimensionCode);
            return new AddressSpaceManager(this, repository);
        }
    }
}
