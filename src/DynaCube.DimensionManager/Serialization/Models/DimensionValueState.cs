using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaCube.Core.Serialization.Models
{
    public class DimensionValueState
    {
        public long Index { get; set; }

        public string DimensionCode { get; set; }

        public string DimensionValueCode { get; set; }

        public string DimensionValueName { get; set; }
    }      
}
