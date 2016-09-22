using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetExtensions
{
  public static class Decimal
  {
    public static decimal TruncateDecimals(this decimal d, int decimalPlaces)
    {
      decimal integralValue = Math.Truncate(d);

      decimal fraction = d - integralValue;

      decimal factor = (decimal)Math.Pow(10, decimalPlaces);

      decimal truncatedFraction = Math.Truncate(fraction * factor) / factor;

      decimal result = integralValue + truncatedFraction;

      return result;
    }
  }
}
