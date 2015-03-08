using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaCube.Core
{
    public class Dimension : GenericDimension<DimensionValue>
    {
        public Dimension(string dimensionCode)
            : base(dimensionCode)
        {

        }
        public Dimension(string dimensionCode, IEnumerable<DimensionValue> values, DimensionValue totalDimension)
            : base(dimensionCode, values, totalDimension)
        {
        }

        protected override DimensionValue Create(string dimensionCode, string dimensionValueCode, string dimensionValueName)
        {
            return new DimensionValue(dimensionCode, dimensionValueCode, dimensionValueName);
        }
    }

    public abstract class GenericDimension<DimensionValueType> : IDimension where DimensionValueType : DimensionValue
    {
        public static string TotalDimensionValueCode = "Null";

        public string DimensionCode { get; private set; }

        protected Dictionary<string, DimensionValueType> _values;
        protected List<string> _keys;
        private SortedList<string, DimensionValueType> _sortedValues;
        private DimensionValueType _totalDimension;

        public GenericDimension(string dimensionCode, DimensionValueType totalDimension)
        {
            Init(dimensionCode, totalDimension, Enumerable.Empty<DimensionValueType>());      
        }        

        public GenericDimension(string dimensionCode)
        {
            Init(dimensionCode, Create(dimensionCode, TotalDimensionValueCode, "Total"), Enumerable.Empty<DimensionValueType>());
        }       

        public GenericDimension(string dimensionCode, IEnumerable<DimensionValueType> values)            
        {
            Init(dimensionCode, Create(dimensionCode, TotalDimensionValueCode, "Total"), values);
        }

        public GenericDimension(string dimensionCode, IEnumerable<DimensionValueType> values, DimensionValueType totalDimension)
        {
            Init(dimensionCode, totalDimension, values);
        }

        private void Init(string dimensionCode, DimensionValueType totalDimension, IEnumerable<DimensionValueType> values)
        {
            this.DimensionCode = dimensionCode;
            this._values = new Dictionary<string, DimensionValueType>();
            this._keys = new List<string>();
            this._sortedValues = new SortedList<string, DimensionValueType>();

            this._totalDimension = totalDimension;

            foreach (var value in values)
            {
                this.AddValue(value);
            }
        }

        protected abstract DimensionValueType Create(string dimensionCode, string dimensionValueCode, string dimensionValueName);

        public void AddValue(DimensionValueType value)
        {
            if (!_values.ContainsKey(value.DimensionValueCode))
            {                               
                // TODO: remove need for this hard coded logic
                string key = value.DimensionValueName + "¬" + value.DimensionValueCode;
                switch (value.DimensionCode)
                {
                    case "ForecastMonth":
                        int i;
                        if (int.TryParse(value.DimensionValueCode, out i))
                        {
                            key = i.ToString("00");
                        }
                        break;                
                }

                _sortedValues.Add(key, value);

                // _keys.Add(value.DimensionValueCode);
                _keys = _sortedValues.Values.Select(v => v.DimensionValueCode).ToList();
            }

            _values[value.DimensionValueCode] = value;
        }

        public DimensionValueType this[string dimensionValueCode]
        {
            get
            {
                if (dimensionValueCode == TotalDimensionValueCode)
                {
                    return _totalDimension;
                }
                else if (_values.ContainsKey(dimensionValueCode))
                {
                    return _values[dimensionValueCode];
                }
                else
                {
                    return null;
                }
            }
        }

        public DimensionValue this[int dimensionValueIndex]
        {
            get
            {                
                return _values[_keys[dimensionValueIndex]];
            }
        }

        DimensionValue IDimension.this[string dimensionValueCode]
        {
            get { return this[dimensionValueCode]; }
        }

        public IEnumerable<DimensionValueType> Values
        {
            get
            {
                return _sortedValues.Values;                
            }
        }

        IEnumerable<DimensionValue> IDimension.Values
        {
            get { return _sortedValues.Values; }
        }

        public long ValueCount
        {
            get
            {
                return _values.Count;
            }
        }

        public override string ToString()
        {
            return DimensionCode;
        }


        public DimensionValue TotalDimension
        {
            get { return _totalDimension; }
        }        
    }    
}
