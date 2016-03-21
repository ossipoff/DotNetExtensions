using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DotNetExtensions.Test
{
    [TestClass]
    public class StringTest
    {
        [TestMethod]
        public void Chunk_returns_correct_chunks_when_input_string_length_is_greater_than_chunk_size()
        {
            string s = "aaabbbcccc";

            var r = s.Chunk(3).ToArray();

            Assert.AreEqual(3, r.Length);
            Assert.AreEqual("aaa", r[0]);
            Assert.AreEqual("bbb", r[1]);
            Assert.AreEqual("ccc", r[2]);
        }

        [TestMethod]
        public void Chunk_returns_a_single_chunk_when_input_string_length_is_less_than_chunk_size()
        {
            string s = "aa";

            var r = s.Chunk(3).ToArray();

            Assert.AreEqual(1, r.Length);
            Assert.AreEqual("aa", r[0]);
        }

        [TestMethod]
        public void Chunk_returns_a_single_chunk_when_input_string_length_is_equal_to_chunk_size()
        {
            string s = "aaa";

            var r = s.Chunk(3).ToArray();

            Assert.AreEqual(1, r.Length);
            Assert.AreEqual("aaa", r[0]);
        }
    }
}
