using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetExtensions.Test
{
    [TestClass]
    public class IEnumerableTest
    {
        private IEnumerable<TestDouble> ienumerable;

        [TestInitialize]
        public void Initialize()
        {
            ienumerable = new TestDouble[]
            {
                new TestDouble()
                {
                    StringProperty = "A",
                    IntProperty = 1,
                    TestDoubleProperty = new TestDouble()
                    {
                        StringProperty = "C",
                        IntProperty = 4
                    }
                },
                new TestDouble()
                {
                    StringProperty = "B",
                    IntProperty = 2,
                    TestDoubleProperty = new TestDouble()
                    {
                        StringProperty = "D",
                        IntProperty = 5
                    }
                },
                new TestDouble()
                {
                    StringProperty = "A",
                    IntProperty = 3,
                    TestDoubleProperty = new TestDouble()
                    {
                        StringProperty = "C",
                        IntProperty = 6
                    }
                },
                new TestDouble()
                {
                    StringProperty = "A",
                    IntProperty = 1,
                    TestDoubleProperty = new TestDouble()
                    {
                        StringProperty = "C",
                        IntProperty = 4
                    }
                }
            };
        }

        [TestMethod]
        public void DistinctBy_performs_correct_equals_comparison_on_int_property()
        {
            var r = ienumerable.DistinctBy(t => t.IntProperty);

            Assert.AreEqual(3, r.Count());
            Assert.AreEqual(1, r.Where(t => t.IntProperty == 1).Count());
            Assert.AreEqual(1, r.Where(t => t.IntProperty == 2).Count());
            Assert.AreEqual(1, r.Where(t => t.IntProperty == 3).Count());
        }

        [TestMethod]
        public void DistinctBy_performs_correct_equals_comparison_on_string_property()
        {
            var r = ienumerable.DistinctBy(t => t.StringProperty);

            Assert.AreEqual(2, r.Count());
            Assert.AreEqual(1, r.Where(t => t.StringProperty == "A").Count());
            Assert.AreEqual(1, r.Where(t => t.StringProperty == "B").Count());
        }

        [TestMethod]
        public void DistinctBy_performs_correct_equals_comparison_on_string_property_in_another_property()
        {
            var r = ienumerable.DistinctBy(t => t.TestDoubleProperty.StringProperty);

            Assert.AreEqual(2, r.Count());
            Assert.AreEqual(1, r.Where(t => t.TestDoubleProperty.StringProperty == "C").Count());
            Assert.AreEqual(1, r.Where(t => t.TestDoubleProperty.StringProperty == "D").Count());
        }

        [TestMethod]
        public void DistinctBy_does_not_throw_exception_when_property_chain_contains_null_reference()
        {
            var i = new TestDouble[]
            {
                new TestDouble()
                {
                },
                new TestDouble()
                {
                    TestDoubleProperty = new TestDouble()
                    {
                        StringProperty = "A"
                    }
                },
                new TestDouble()
                {
                    TestDoubleProperty = new TestDouble()
                    {
                        StringProperty = "A"
                    }
                }
            };

            var r = i.DistinctBy(t => t.TestDoubleProperty.StringProperty);

            Assert.AreEqual(2, r.Count());
        }
    }

    class TestDouble
    {
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }

        public TestDouble TestDoubleProperty { get; set; }
    }
}
