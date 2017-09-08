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

        [TestMethod]
        public void Truncate_returns_null_when_passed_null()
        {
            string s = null;

            var r = s.Truncate(10);

            Assert.AreEqual(null, r);
        }

        [TestMethod]
        public void Truncate_returns_correctly_truncated_string_and_defaults_to_ellipsis_as_omission_indicator()
        {
            string s = "Lorem ipsum dolor sit amet";

            var r = s.Truncate(10);

            Assert.AreEqual("Lorem ipsu…", r);
        }

        [TestMethod]
        public void Truncate_returns_correctly_truncated_string_and_uses_passed_omission_indicator_char()
        {
            string s = "Lorem ipsum dolor sit amet";

            var r = s.Truncate(10, (char)8942);

            Assert.AreEqual("Lorem ipsu⋮", r);
        }

        [TestMethod]
        public void Truncate_returns_correctly_truncated_string_and_uses_passed_omission_indicator_string()
        {
            string s = "Lorem ipsum dolor sit amet";

            var r = s.Truncate(10, " (...)");

            Assert.AreEqual("Lorem ipsu (...)", r);
        }

        [TestMethod]
        public void LatinDiacriticsToAscii_returns_string_where_latin_characters_are_replace_by_ascii_characters()
        {
            string s = "À à Á á Â â Ã ã Ä ä Å å Æ æ Ç ç È è É é Ê ê Ë ë Ì ì Í í Î î Ï ï ı Ñ ñ Ò ò Ó ó Ô ô Õ õ Ö ö Ø ø Œ œ Š š ẞ ß Ù ù Ú ú Û û Ü ü Ý ý Ÿ ÿ Ž ž Ð ð Þ þ";
            string e = "A a A a A a A a Ae ae Aa aa Ae ae C c E e E e E e E e I i I i I i I i i N n O o O o O o O o Oe oe Oe oe Oe oe S s S s U u U u U u U u Y y Y y Z z Th th Eth eth";

            var r = s.LatinDiacriticsToAscii();

            Assert.AreEqual(e, r);
        }

        [TestMethod]
        public void Slugify_correctly_turns_latin_string_with_spaces_and_upper_and_lowercase_characters_into_a_slug()
        {
            string s = "A a B b C c D d E e F f G g H h I i J j K k L l M m N n O o P p Q q R r S s T t U u V v W w X x Y y Z z";

            var r = s.Slugify();

            Assert.AreEqual("a-a-b-b-c-c-d-d-e-e-f-f-g-g-h-h-i-i-j-j-k-k-l-l-m-m-n-n-o-o-p-p-q-q-r-r-s-s-t-t-u-u-v-v-w-w-x-x-y-y-z-z", r);
        }

        [TestMethod]
        public void Slugify_correctly_turns_latin_diacritic_string_with_spaces_and_upper_and_lowercase_characters_into_a_slug()
        {
            string s = "À à Á á Â â Ã ã Ä ä Å å Æ æ Ç ç È è É é Ê ê Ë ë Ì ì Í í Î î Ï ï ı Ñ ñ Ò ò Ó ó Ô ô Õ õ Ö ö Ø ø Œ œ Š š ẞ ß Ù ù Ú ú Û û Ü ü Ý ý Ÿ ÿ Ž ž Ð ð Þ þ";
            string e = "a-a-a-a-a-a-a-a-ae-ae-aa-aa-ae-ae-c-c-e-e-e-e-e-e-e-e-i-i-i-i-i-i-i-i-i-n-n-o-o-o-o-o-o-o-o-oe-oe-oe-oe-oe-oe-s-s-s-s-u-u-u-u-u-u-u-u-y-y-y-y-z-z-th-th-eth-eth";

            var r = s.Slugify();

            Assert.AreEqual(e, r);
        }

        [TestMethod]
        public void Slugify_correctly_turns_numeric_string_with_spaces_and_dots_into_a_slug()
        {
            string s = "0 1 2 3 4 5 6 7 8 9 1.0";
            string e = "0-1-2-3-4-5-6-7-8-9-1.0";

            var r = s.Slugify();

            Assert.AreEqual(e, r);
        }

        [TestMethod]
        public void Slugify_correctly_turns_non_alphanumeric_string_with_spaces_into_a_slug()
        {
            string s = "½ + ' < , . - § ! \" # ¤ % & / ( ) = * > ; : @ £ $ € { [ ] } | \\ µ";
            string e = "-----.----------------------------";

            var r = s.Slugify();

            Assert.AreEqual(e, r);
        }
    }
}
