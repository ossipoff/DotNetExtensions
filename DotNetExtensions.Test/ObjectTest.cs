using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetExtensions.Test
{
    [TestClass]
    public class ObjectTest
    {
        [TestMethod]
        public void AllEqual_returns_true_with_identical_strings_using_default_EqualityComparer()
        {
            var r = "a".AllEqual("a", "a", "a");

            Assert.IsTrue(r);
        }

        [TestMethod]
        public void AllEqual_returns_false_with_different_strings_using_default_EqualityComparer()
        {
            var r = "a".AllEqual("a", "b", "c");

            Assert.IsFalse(r);
        }

        [TestMethod]
        public void AllEqual_uses_the_passed_custom_EqualityComparer()
        {
            var stringLengthComparer = new StringLengthComparer();

            var r = "a".AllEqual(stringLengthComparer, "a", "b", "c");

            Assert.IsTrue(r);
            Assert.AreEqual(3, stringLengthComparer.CallsToEqualsMethod);
        }

        [TestMethod]
        public void GetMemberName_returns_correct_name_for_value_type_property()
        {
            var r = GetMemberNameTestDouble.Instance.GetMemberName(o => o.IntProperty);

            Assert.AreEqual("IntProperty", r);
        }

        [TestMethod]
        public void GetMemberName_returns_correct_name_for_reference_type_property()
        {
            var r = GetMemberNameTestDouble.Instance.GetMemberName(o => o.StringProperty);

            Assert.AreEqual("StringProperty", r);
        }

        [TestMethod]
        public void GetMemberName_returns_correct_name_for_method_with_return_type()
        {
            var r = GetMemberNameTestDouble.Instance.GetMemberName(o => o.StringMethod());

            Assert.AreEqual("StringMethod", r);
        }

        [TestMethod]
        public void GetMemberName_returns_correct_name_for_method_without_return_type()
        {
            var r = GetMemberNameTestDouble.Instance.GetMemberName(o => o.VoidMethod());

            Assert.AreEqual("VoidMethod", r);
        }

        [TestMethod]
        public void GetMemberValue_returns_correct_value_for_value_type_property()
        {
            var testDouble = new GetMemberValueTestDouble()
            {
                IntProperty = 1
            };

            var r = testDouble.GetMemberValue(o => o.IntProperty);

            Assert.AreEqual(testDouble.IntProperty, r);
        }

        [TestMethod]
        public void GetMemberValue_returns_correct_value_for_reference_type_property()
        {
            var testDouble = new GetMemberValueTestDouble()
            {
                StringProperty = "abc"
            };

            var r = testDouble.GetMemberValue(o => o.StringProperty);

            Assert.AreEqual(testDouble.StringProperty, r);
        }
    }

    public class GetMemberValueTestDouble
    {
        public int IntProperty { get; set; }

        public string StringProperty { get; set; }
    }

    public class GetMemberNameTestDouble
    {
        public int IntProperty { get; set; }

        public string StringProperty { get; set; }

        public void VoidMethod()
        {
        }

        public string StringMethod()
        {
            return null;
        }

        public static GetMemberNameTestDouble Instance = new GetMemberNameTestDouble();
    }

    public class StringLengthComparer : EqualityComparer<string>
    {
        public int CallsToEqualsMethod { get; set; }

        public override bool Equals(string x, string y)
        {
            CallsToEqualsMethod++;
            return string.Equals(x.Length, y.Length);
        }

        public override int GetHashCode(string obj)
        {
            return obj.Length;
        }
    }
}
