using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaCube.Core
{
    public class DimensionValue
    {        
        public string DimensionCode { get; private set; }
        public string DimensionValueCode { get; private set; }
        public string DimensionValueName { get; private set; }

        public DimensionValue(string dimensionCode, string dimensionValueCode, string dimensionValueName)
        {
            if (String.IsNullOrWhiteSpace(dimensionCode))
            {
                throw new Exception("DimensionValue DimensionCode missing");
            }
            if (String.IsNullOrWhiteSpace(dimensionValueCode))
            {
                throw new Exception("DimensionValue DimensionValueCode missing");
            }

            this.DimensionCode = dimensionCode;
            this.DimensionValueCode = dimensionValueCode;
            this.DimensionValueName = dimensionValueName ?? dimensionValueCode;
        }
    
        public override string ToString()
        {
            if (DimensionValueName != DimensionValueCode)
            {
                return string.Format("{0} ({1})", DimensionValueName, DimensionValueCode);
            }
            else
            {
                return string.Format("{0}", DimensionValueName);
            }
        }

        public override int GetHashCode()
        {
            return (DimensionCode + "¬" + DimensionValueCode).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var dv2 = obj as DimensionValue;

            if (dv2 == null)
            {
                return base.Equals(obj);
            }

            return 
                this.DimensionCode == dv2.DimensionCode &&
                this.DimensionValueCode == dv2.DimensionValueCode;

        }
    }
}
