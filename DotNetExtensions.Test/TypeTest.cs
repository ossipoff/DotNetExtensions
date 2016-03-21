using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetExtensions.Test
{
    [TestClass]
    public class TypeTest
    {
        [TestMethod]
        public void GetCustomAttributes_return_list_with_1_element_when_class_has_1_custom_attribute_of_a_specific_type()
        {
            IEnumerable<CustomAttributeTestDoubleAttribute> result = typeof(GetCustomAttributesTestDoubleOneAttribute).GetCustomAttributes<CustomAttributeTestDoubleAttribute>();
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void GetCustomAttributes_return_list_with_2_element_when_class_has_2_custom_attribute_of_a_specific_type()
        {
            IEnumerable<CustomAttributeTestDoubleAttribute> result = typeof(GetCustomAttributesTestDoubleTwoAttributes).GetCustomAttributes<CustomAttributeTestDoubleAttribute>();
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetCustomAttributes_does_not_find_custom_attributes_in_decendants_that_are_marked_Inherited_when_AttributeSearchMode_equals_TypeOnly()
        {
            IEnumerable<CustomAttributeTestDoubleInheritedAttribute> result = typeof(SubGetCustomAttributesTestDouble).GetCustomAttributes<CustomAttributeTestDoubleInheritedAttribute>(Type.AttributeSearchMode.TypeOnly);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetCustomAttributes_does_not_find_custom_attributes_in_decendants_that_are_not_marked_Inherited_when_AttributeSearchMode_equals_TypeOnly()
        {
            IEnumerable<CustomAttributeTestDoubleAttribute> result = typeof(SubGetCustomAttributesTestDouble).GetCustomAttributes<CustomAttributeTestDoubleAttribute>(Type.AttributeSearchMode.TypeOnly);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetCustomAttributes_does_not_find_custom_attributes_in_decendants_that_are_not_marked_Inherited_when_AttributeSearchMode_equals_InheritanceChain()
        {
            IEnumerable<CustomAttributeTestDoubleAttribute> result = typeof(SubGetCustomAttributesTestDouble).GetCustomAttributes<CustomAttributeTestDoubleAttribute>(Type.AttributeSearchMode.InheritanceChain);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetCustomAttributes_finds_custom_attributes_in_decendants_that_are_not_marked_Inherited_when_AttributeSearchMode_equals_InheritanceChainForced()
        {
            IEnumerable<CustomAttributeTestDoubleAttribute> result = typeof(SubGetCustomAttributesTestDouble).GetCustomAttributes<CustomAttributeTestDoubleAttribute>(Type.AttributeSearchMode.InheritanceChainForced);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void GetCustomAttributes_finds_custom_attributes_indecendants_that_are_marked_Inherited_when_AttributeSearchMode_equals_InheritanceChain()
        {
            IEnumerable<CustomAttributeTestDoubleInheritedAttribute> result = typeof(SubGetCustomAttributesTestDouble).GetCustomAttributes<CustomAttributeTestDoubleInheritedAttribute>(Type.AttributeSearchMode.InheritanceChain);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }
    }

    [CustomAttributeTestDouble]
    [CustomAttributeTestDoubleInherited]
    public class BaseGetCustomAttributesTestDouble
    {

    }

    public class SubGetCustomAttributesTestDouble : BaseGetCustomAttributesTestDouble
    {
    }

    [CustomAttributeTestDouble]
    [CustomAttributeTestDoubleInherited]
    public class GetCustomAttributesTestDoubleOneAttribute
    {
    }

    [CustomAttributeTestDouble]
    [CustomAttributeTestDouble]
    [CustomAttributeTestDoubleInherited]
    [CustomAttributeTestDoubleInherited]
    public class GetCustomAttributesTestDoubleTwoAttributes
    {
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class CustomAttributeTestDoubleAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class CustomAttributeTestDoubleInheritedAttribute : Attribute
    {
    }
}
