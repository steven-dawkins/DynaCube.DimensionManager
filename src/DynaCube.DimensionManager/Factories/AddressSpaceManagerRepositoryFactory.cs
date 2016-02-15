using DynaCube.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaCube.Core.Factories
{
    public interface AddressSpaceManagerRepositoryFactory
    {
        IAddressSpaceManagerRepository GetAddressSpaceManagerRepository(string dimensionCode);
    }

    public class InMemoryAddressSpaceManagerFactory : AddressSpaceManagerRepositoryFactory
    {
        public IAddressSpaceManagerRepository GetAddressSpaceManagerRepository(string dimensionCode)
        {
            return new InMemoryAddressSpaceManagerRepository();
        }
    }
}
