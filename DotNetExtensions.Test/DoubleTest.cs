using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetExtensions.Test
{
  [TestClass]
  public class DoubleTest
  {
    [TestMethod]
    public void TruncateDecimals_returns_unrounded_result()
    {
      double d = 123.139;
      var r = d.TruncateDecimals(2);
      Assert.AreEqual(123.13, r);
    }
  }
}
