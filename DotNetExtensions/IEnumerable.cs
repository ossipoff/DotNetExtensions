using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetExtensions
{
    public static class IEnumerable
    {
        private static Regex naturalOrderRegEx = new System.Text.RegularExpressions.Regex($@"(\d*\{Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator}?\d+)|([^\d]*)");

        public static IEnumerable<T> DistinctBy<T>(this IEnumerable<T> ienumerable, Expression<Func<T, object>> property)
        {
            var valueFunc = property.Compile();

            var distinctByComparer = new DistinctByComparer<T>(valueFunc);

            return ienumerable.Distinct(distinctByComparer);
        }

        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> ienumerable, int chunkSize)
        {
            return ienumerable
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public static IOrderedEnumerable<T> OrderByNatural<T>(this IEnumerable<T> ienumerable, Func<T, string> selector)
        {
            var items = ienumerable.Select(i => new { Item = i, Text = selector(i) }).Select(s => new {
                Item = s.Item,
                TextColumns = naturalOrderRegEx.Matches(s.Text).OfType<Match>().Select(m => m.Captures[0].Value).Where(x => !string.IsNullOrEmpty(x)).ToArray()
            });

            var maxColumnCount = items.Max(l => l.TextColumns.Count());

            items = items.Select(s =>
            {
                string[] a = new string[maxColumnCount];
                s.TextColumns.CopyTo(a, 0);
                return new { Item = s.Item, TextColumns = a };
            });

            var orderedItems = items.OrderBy(x => x.TextColumns[0], new NumericStringComparer());

            for (int i = 1; i < maxColumnCount; i++)
            {
                int columnIndex = i; //avoids out of bounds exception
                orderedItems = orderedItems.ThenBy(x => x.TextColumns[columnIndex], new NumericStringComparer());
            }

            return orderedItems.Select(x => x.Item).OrderBy(x => 1);
        }

        public static IOrderedEnumerable<T> OrderByNaturalDescending<T>(this IEnumerable<T> ienumerable, Func<T, string> selector)
        {
            return ienumerable.OrderByNatural(selector).Reverse().OrderBy(x => 1);
        }

        private class NumericStringComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                if (double.TryParse(x, out double xd) && double.TryParse(y, out double yd))
                {
                    return xd.CompareTo(yd);
                }

                return string.Compare(x, y);
            }
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
