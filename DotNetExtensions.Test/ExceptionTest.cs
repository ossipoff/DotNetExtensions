using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetExtensions.Test
{
    [TestClass]
    public class ExceptionTest
    {
        [TestMethod]
        public void GetInnerExceptions_returns_list_of_inner_exceptions()
        {
            var ex1 = new System.Exception("ex1");
            var ex2 = new System.Exception("ex2", ex1);
            var ex3 = new System.Exception("ex3", ex2);

            var r = ex3.GetInnerExceptions().ToArray();

            Assert.AreEqual(2, r.Count());
            CollectionAssert.Contains(r, ex1);
            CollectionAssert.Contains(r, ex2);
        }

        [TestMethod]
        public void GetInnermostException_returns_innermost_exception()
        {
            var ex1 = new System.Exception("ex1");
            var ex2 = new System.Exception("ex2", ex1);
            var ex3 = new System.Exception("ex3", ex2);

            var r = ex3.GetInnermostException();

            Assert.AreEqual(ex1, r);
        }

        [TestMethod]
        public void ToStringAdvanced_uses_expected_default_format_strings()
        {
            var ex1 = new System.Exception("ex1");
            var r = ex1.ToStringAdvanced();

            var exceptionFormat = "{0}:" + Environment.NewLine + "{1}";
            var propertyFormat = "{0} = {1}" + Environment.NewLine;
            var expected = string.Format(exceptionFormat, ex1.GetType().FullName, string.Format(propertyFormat, "Message", ex1.Message)).Trim();

            Assert.AreEqual(expected, r);
        }

        [TestMethod]
        public void ToStringAdvanced_includes_inner_exceptions()
        {
            var ex1 = new System.Exception("ex1");
            var ex2 = new System.Exception("ex2", ex1);
            var r = ex2.ToStringAdvanced();

            StringAssert.Contains(r, ex1.Message);
            StringAssert.Contains(r, ex2.Message);
        }

        [TestMethod]
        public void ToStringAdvanced_uses_expected_default_property_filter()
        {
            try
            {
                throw new System.Exception("ex1", new System.Exception());
            }
            catch (System.Exception ex)
            {
                var r = ex.ToStringAdvanced();

                StringAssert.Contains(r, "Message");
                StringAssert.Contains(r, "StackTrace");
                StringAssert.Contains(r, "Source");
                StringAssert.Contains(r, "TargetSite");
                StringAssert.Contains(r, "InnerException");
            }
        }
    }
}
