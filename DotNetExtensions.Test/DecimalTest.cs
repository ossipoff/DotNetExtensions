using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetExtensions.Test
{
  [TestClass]
  public class DecimalTest
  {
    [TestMethod]
    public void TruncateDecimals_returns_unrounded_result()
    {
      decimal d = 123.139M;
      var r = d.TruncateDecimals(2);
      Assert.AreEqual(123.13M, r);
    }
  }
}
