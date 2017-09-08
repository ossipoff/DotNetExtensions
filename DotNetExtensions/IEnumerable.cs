using System;
using System.Collections.Generic;
using System.Globalization;
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
        private static readonly string naturalOrderRegexString = @"([+-]?\d*{0}?\d+)|([^\d^+^-]*|[+-]*)";
        
        private static readonly Dictionary<string, Regex> naturalOrderRegexes = new Dictionary<string, System.Text.RegularExpressions.Regex>()
        {
            { ".", new Regex(string.Format(naturalOrderRegexString, "\\."), RegexOptions.Compiled) },
            { ",", new Regex(string.Format(naturalOrderRegexString, ","), RegexOptions.Compiled) }
        };
        
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

        public static IOrderedEnumerable<T> OrderByNatural<T>(this IEnumerable<T> ienumerable, Func<T, string> selector, NumberFormatInfo numberFormat = null)
        {
            if (ienumerable.Count() == 0)
            {
                return ienumerable.OrderBy(x => 1);
            }

            if (numberFormat == null)
            {
                numberFormat = Thread.CurrentThread.CurrentCulture.NumberFormat;
            }

            var decimalSeparator = numberFormat.NumberDecimalSeparator;

            if (!naturalOrderRegexes.TryGetValue(decimalSeparator, out Regex naturalOrderRegex))
            {
                naturalOrderRegex = naturalOrderRegexes.Values.First();
            }
            
            var items = ienumerable.Select(i => new { Item = i, Text = selector(i) }).Select(s => new {
                Item = s.Item,
                TextColumns = naturalOrderRegex.Matches(s.Text).OfType<Match>().Select(m => m.Captures[0].Value).Where(x => !string.IsNullOrEmpty(x)).ToArray()
            });

            var maxColumnCount = items.Max(l => l.TextColumns.Count());

            items = items.Select(s =>
            {
                string[] a = new string[maxColumnCount];
                s.TextColumns.CopyTo(a, 0);
                return new { Item = s.Item, TextColumns = a };
            });

            var numbericStringComparer = new NumericStringComparer(numberFormat);

            var orderedItems = items.OrderBy(x => x.TextColumns[0], numbericStringComparer);

            for (int i = 1; i < maxColumnCount; i++)
            {
                int columnIndex = i; //avoids out of bounds exception
                orderedItems = orderedItems.ThenBy(x => x.TextColumns[columnIndex], numbericStringComparer);
            }

            return orderedItems.Select(x => x.Item).OrderBy(x => 1);
        }

        public static IOrderedEnumerable<T> OrderByNaturalDescending<T>(this IEnumerable<T> ienumerable, Func<T, string> selector)
        {
            return ienumerable.OrderByNatural(selector).Reverse().OrderBy(x => 1);
        }

        private class NumericStringComparer : IComparer<string>
        {
            private readonly NumberFormatInfo numberFormat;

            public NumericStringComparer(NumberFormatInfo numberFormat)
            {
                this.numberFormat = numberFormat;
            }

            public int Compare(string x, string y)
            {
                if (double.TryParse(x, NumberStyles.Number, numberFormat, out double xd) && double.TryParse(y, NumberStyles.Number, numberFormat, out double yd))
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
