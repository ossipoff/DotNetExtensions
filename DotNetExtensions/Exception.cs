using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetExtensions
{
    public static class Exception
    {
        public static IEnumerable<System.Exception> GetInnerExceptions(this System.Exception exception)
        {
            if (exception.InnerException != null)
            {
                foreach (var ex in GetInnerExceptions(exception.InnerException))
                {
                    yield return ex;
                }

                yield return exception.InnerException;
            }
        }

        public static System.Exception GetInnermostException(this System.Exception exception)
        {
            if (exception.InnerException != null)
            {
                return exception.InnerException.GetInnermostException();
            }

            return exception;
        }

        public static string ToStringAdvanced(this System.Exception exception, string exceptionFormat = null, string propertyFormat = null, IEnumerable<string> propertyFilter = null, string indentationString = "  ")
        {
            return exception.ToStringAdvanced(exceptionFormat, propertyFormat, propertyFilter, indentationString, 0).Trim();
        }

        private static string ToStringAdvanced(this System.Exception exception, string exceptionFormat, string propertyFormat, IEnumerable<string> propertyFilter, string indentationString, int depth)
        {
            if (exception == null)
            {
                return null;
            }

            exceptionFormat = exceptionFormat ?? Environment.NewLine + "{0}:" + Environment.NewLine + "{1}";
            propertyFormat = propertyFormat ?? "{0} = {1}" + Environment.NewLine;
            propertyFilter = propertyFilter ?? new string[] { "Message", "StackTrace", "Source", "TargetSite", "InnerException" };

            var properties = exception.GetType().GetProperties().Select(p => new KeyValuePair<string, string>(p.Name, Convert.ToString(p.GetValue(exception)))).ToDictionary(p => p.Key, p => p.Value);

            var filteredProperties = properties.Where(p => propertyFilter.Contains(p.Key) && !string.IsNullOrEmpty(p.Value)).ToDictionary(p => p.Key, p => p.Value);

            if (filteredProperties.ContainsKey("InnerException"))
            {
                filteredProperties["InnerException"] = exception.InnerException.ToStringAdvanced(exceptionFormat, propertyFormat, propertyFilter, indentationString, depth + 1);
            }

            var formatedProperties = filteredProperties.Select(p => string.Format(propertyFormat, p.Key, p.Value)).ToList();

            var detailString = string.Format(exceptionFormat, exception.GetType().FullName, string.Join("", formatedProperties));

            if (!string.IsNullOrEmpty(indentationString) && depth > 0)
            {
                string indentation = string.Concat(Enumerable.Repeat(indentationString, depth));

                var lines = detailString.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                detailString = Environment.NewLine + string.Join(Environment.NewLine, lines.Select(l => indentation + l));
            }

            return detailString;
        }
    }
}
