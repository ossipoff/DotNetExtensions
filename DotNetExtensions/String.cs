using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
