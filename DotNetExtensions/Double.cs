using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetExtensions
{
  public static class Double
  {
    public static double TruncateDecimals(this double d, int decimalPlaces)
    {
      return (double)((decimal)d).TruncateDecimals(decimalPlaces);
    }
  }
}
