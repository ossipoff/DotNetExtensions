using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetExtensions
{
	public static class Enum
	{
		public static TEnum Parse<TEnum>(object value, TEnum unknown = default(TEnum)) where TEnum : struct
		{
			return ParseNullable(value, (TEnum?)unknown).GetValueOrDefault();
		}

		public static TEnum? ParseNullable<TEnum>(object value, TEnum? unknown = default(TEnum?)) where TEnum : struct
		{
			TEnum? result = unknown;

			if (value != null)
			{
				try
				{
					result = (TEnum)System.Enum.Parse(typeof(TEnum), value.ToString());
				}
				catch (System.Exception)
				{
					result = unknown;
				}
			}

			return result;
		}

        public static IEnumerable<decimal> GetIntegralValues<TEnum>()
        {
            var tEnum = typeof(TEnum);
            var tIntegral = System.Enum.GetUnderlyingType(tEnum);

            return System.Enum.GetValues(tEnum).OfType<TEnum>().Select(e => (decimal)Convert.ChangeType(e, typeof(decimal)));
        }
    }
}
