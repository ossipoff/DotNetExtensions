using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotNetExtensions
{
    public static class String
    {
        public static IEnumerable<string> Chunk(this string s, int chunkSize)
        {
            if (s.Length > chunkSize)
            {
                return Enumerable.Range(0, s.Length / chunkSize).Select(i => s.Substring(i * chunkSize, chunkSize)).ToArray();
            }
            else
            {
                return new string[] { s };
            }
        }

        public static string Truncate(this string value, int maxLength, char omissionIndicator = (char)8230)
        {
            return Truncate(value, maxLength, omissionIndicator.ToString());
        }

        public static string Truncate(this string value, int maxLength, string omissionIndicator)
        {
            return (value?.Length).GetValueOrDefault() <= maxLength ? value : $"{value.Substring(0, maxLength)}{omissionIndicator}";
        }

        public static Dictionary<char, string> latinDiacriticsToAsciiReplacements = new Dictionary<char, string>()
        {
            { 'ı', "i" },
            { 'ẞ', "S" },
            { 'ß', "s" },
            { 'Ð', "Th" },
            { 'ð', "th" },
            { 'Þ', "Eth" },
            { 'þ', "eth" },
            { 'Ä', "Ae" },
            { 'ä', "ae" },
            { 'Æ', "Ae" },
            { 'æ', "ae" },
            { 'Ö', "Oe" },
            { 'ö', "oe" },
            { 'Ø', "Oe" },
            { 'ø', "oe" },
            { 'Œ', "Oe" },
            { 'œ', "oe" },
            { 'Å', "Aa" },
            { 'å', "aa" }
        };

        public static string LatinDiacriticsToAscii(this string text)
        {
            foreach (var replacement in latinDiacriticsToAsciiReplacements)
            {
                text = text.Replace(replacement.Key.ToString(), replacement.Value);
            }

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private static readonly Regex slugifyRegex = new Regex(@"[^a-zA-Z0-9\-\.]");

        private static Dictionary<string, string> slugifyReplacements = new Dictionary<string, string>()
        {
          { " ", "-" }
        };

        public static string Slugify(this string text)
        {
            text = text.ToLower();

            foreach (var replacement in slugifyReplacements)
            {
                text = text.Replace(replacement.Key.ToString(), replacement.Value);
            }

            text = text.LatinDiacriticsToAscii().Replace(" ", "-");

            return slugifyRegex.Replace(text, "");
        }
    }
}
