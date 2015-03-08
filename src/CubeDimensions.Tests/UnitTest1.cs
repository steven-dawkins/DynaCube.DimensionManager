using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApprovalTests;
using ApprovalTests.Reporters;
using System.Collections.Generic;

namespace CubeDimensions.Tests
{
    [UseReporter(typeof(DiffReporter))]
    [TestClass]
    public class UnitTest1
    {
        DimensionValue gbr = new DimensionValue("Location", "GBR");
        DimensionValue fra = new DimensionValue("Location", "FRA");
        DimensionValue deu = new DimensionValue("Location", "DEU");

        DimensionValue gdp = new DimensionValue("Indicator", "GDP");
        DimensionValue cpi = new DimensionValue("Indicator", "CPI");
        DimensionValue emp = new DimensionValue("Indicator", "EMP");

        [TestMethod]
        public void GetAllIndicators()
        {
            var c = CreateCubeDimensions();

            var allIndicators = c.Enumerate(new DimensionValue[] {  }, "Indicator");

            Approvals.Verify(String.Join(", ", allIndicators));
        }

        [TestMethod]
        public void GetGbrIndicators()
        {
            var c = CreateCubeDimensions();

            var gbrIndicators = c.Enumerate(new[] { gbr }, "Indicator");
            
            Approvals.Verify(String.Join(", ", gbrIndicators));            
        }

        [TestMethod]
        public void GetDeuIndicators()
        {
            var c = CreateCubeDimensions();

            var deuIndicators = c.Enumerate(new[] { deu }, "Indicator");

            Approvals.Verify(String.Join(", ", deuIndicators));
        }

        [TestMethod]
        public void GetCpiLocations()
        {
            var c = CreateCubeDimensions();
            
            var cpiCountries = c.Enumerate(new[] { cpi }, "Location");
            
            Approvals.Verify(String.Join(", ", cpiCountries));
        }

        private CubeDimensions2 CreateCubeDimensions()
        {
            var c = new CubeDimensions2();

            c.AddPoint(new[] { gbr, gdp });
            c.AddPoint(new[] { gbr, cpi });
            c.AddPoint(new[] { gbr, emp });

            c.AddPoint(new[] { fra, cpi });
            c.AddPoint(new[] { fra, emp });

            c.AddPoint(new[] { deu, emp });
            return c;
        }

        [TestMethod]
        public void BigTest()
        {
            var c = CreateDimensions();           
        }

        [TestMethod]
        public void BigTest2()
        {
            var c = CreateDimensions();
        }

        private CubeDimensions2 CreateDimensions()
        {
            var c = new CubeDimensions2();

            Dictionary<string, List<DimensionValue>> dimensions = new Dictionary<string, List<DimensionValue>>();


            for(var i = 0; i < 5; i++)
            {
                var dimensionCode = "Dimension" + i;
                dimensions.Add(dimensionCode, new List<DimensionValue>());

                for (var j = 0; j < 1000; j++)
                {
                    dimensions[dimensionCode].Add(new DimensionValue(dimensionCode, dimensionCode + "_" + j));
                }
            }

            var rand = new Random();
            for (var i = 0; i < 1000000; i++)
            {
                var point = new List<DimensionValue>();
                
                foreach(var dimension in dimensions.Values)
                {
                    var index = rand.Next(dimension.Count);
                    point.Add(dimension[index]);
                }

                c.AddPoint(point);
            }

            return c;
        }
    }
}
