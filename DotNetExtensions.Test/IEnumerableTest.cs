using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

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

        [TestMethod]
        public void Chunk_splits_list_of_strings()
        {
            var stringList = new[] { "A", "B", "C", "D" };

            var r = stringList.Chunk(2);

            Assert.AreEqual(2, r.Count());
            Assert.AreEqual(2, r.First().Count());
            Assert.AreEqual(2, r.Last().Count());
            Assert.AreEqual("AB", string.Join("", r.First()));
            Assert.AreEqual("CD", string.Join("", r.Last()));
        }

        [TestMethod]
        public void OrderByNatural_orders_list_of_integer_strings_correctly()
        {
            var stringList = new[] { "1", "10", "2", "20" };

            var r = stringList.OrderByNatural(s => s);

            Assert.AreEqual("1,2,10,20", string.Join(",", r));
        }

        [TestMethod]
        public void OrderByNatural_orders_list_of_signed_integer_strings_correctly()
        {
            var stringList = new[] { "1", "-10", "2", "-20" };

            var r = stringList.OrderByNatural(s => s);

            Assert.AreEqual("-20,-10,1,2", string.Join(",", r));
        }

        [TestMethod]
        public void OrderByNatural_orders_list_of_signed_integer_mixed_strings_correctly()
        {
            var stringList = new[] { "10 abc -30 def", "1", "-10 abc +20 def", "2", "-10 abc -30 def" };

            var r = stringList.OrderByNatural(s => s);

            Assert.AreEqual("-10 abc -30 def,-10 abc +20 def,1,2,10 abc -30 def", string.Join(",", r));
        }

        [TestMethod]
        public void OrderByNatural_orders_list_with_plusses_and_minusses_correctly()
        {
            var stringList = new[] { "ABC---++DEF-1", "ABC---++DEF1", "ABC---++GHI-2", "ABC---++DEF-10", "ABC---++DEF-20" };

            var r = stringList.OrderByNatural(s => s);

            Assert.AreEqual("ABC---++DEF-20,ABC---++DEF-10,ABC---++DEF-1,ABC---++DEF1,ABC---++GHI-2", string.Join(",", r));
        }

        [TestMethod]
        public void OrderByNatural_orders_list_of_floating_point_strings_correctly()
        {
            var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var stringList = new[] { "1.0", "1.10", "2.01", "2.10" };

            var r = stringList.OrderByNatural(s => s);

            Assert.AreEqual("1.0,1.10,2.01,2.10", string.Join(",", r));

            System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
        }

        [TestMethod]
        public void OrderByNatural_orders_list_of_mixed_content_strings_correctly()
        {
            var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var stringList = new[] { "1.10 DE", "1.11 AB", "1.10 CD", "1.20 BC" };

            var r = stringList.OrderByNatural(s => s);

            Assert.AreEqual("1.10 CD,1.10 DE,1.11 AB,1.20 BC", string.Join(",", r));

            System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
        }

        [TestMethod]
        public void OrderByNaturalDescending_orders_list_of_integer_strings_correctly()
        {
            var stringList = new[] { "1", "10", "2", "20" };

            var r = stringList.OrderByNaturalDescending(s => s);

            Assert.AreEqual("20,10,2,1", string.Join(",", r));
        }

        [TestMethod]
        public void OrderByNatural_handles_empty_IEnumerable_without_throwing_exception()
        {
            var stringList = new string[] { };

            var r = stringList.OrderByNatural(s => s);

            Assert.AreEqual(0, r.Count());
        }
    }

    class TestDouble
    {
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }

        public TestDouble TestDoubleProperty { get; set; }
    }
}
