using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DotNetExtensions
{
    public static class IEnumerable
    {
        public static IEnumerable<T> DistinctBy<T>(this IEnumerable<T> ienumerable, Expression<Func<T, object>> property)
        {
            var valueFunc = property.Compile();

            var distinctByComparer = new DistinctByComparer<T>(valueFunc);

            return ienumerable.Distinct(distinctByComparer);
        }

        private class DistinctByComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, object> valueFunc;

            public DistinctByComparer(Func<T, object> valueFunc)
            {
                this.valueFunc = valueFunc;
            }

            public bool Equals(T x, T y)
            {
                object xValue, yValue;

                try
                {
                    xValue = valueFunc(x);
                }
                catch (NullReferenceException)
                {
                    xValue = null;
                }

                try
                {
                    yValue = valueFunc(y);
                }
                catch (NullReferenceException)
                {
                    yValue = null;
                }

                if (xValue != null && yValue != null)
                {
                    return xValue.Equals(yValue);
                }

                return xValue == yValue;
            }

            public int GetHashCode(T obj)
            {
                try
                {
                    var value = valueFunc(obj);
                    return value.GetHashCode();
                }
                catch (NullReferenceException)
                {
                    return 0;
                }
            }
        }
    }
}
