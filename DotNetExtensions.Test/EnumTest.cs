using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetExtensions.Test
{
	[TestClass]
	public class EnumTest
	{
		[TestMethod]
		public void Parse_correctly_parses_string()
		{
			DummyEnum r = DotNetExtensions.Enum.Parse<DummyEnum>("B");

			Assert.AreEqual(DummyEnum.B, r);
        }

		[TestMethod]
		public void Parse_correctly_parses_int()
		{
			DummyEnum r = DotNetExtensions.Enum.Parse<DummyEnum>(2);

			Assert.AreEqual(DummyEnum.C, r);
		}

		[TestMethod]
		public void Parse_returns_default_when_parse_fails()
		{
            DummyEnum r = DotNetExtensions.Enum.Parse<DummyEnum>("D", DummyEnum.B);

			Assert.AreEqual(DummyEnum.B, r);
		}

		[TestMethod]
		public void ParseNullable_returns_null_when_parse_fails()
		{
			DummyEnum? r = DotNetExtensions.Enum.ParseNullable<DummyEnum>("D");

			Assert.AreEqual(null, r);
		}

        [TestMethod]
        public void GetIntegralValues_returns_correct_values()
        {
            var r = Enum.GetIntegralValues<DummyEnum>().ToArray();

            CollectionAssert.AreEqual(new decimal[] { 0, 1, 2 }, r);
        }

        [TestMethod]
        public void GetIntegralValues_returns_correct_values_with_byte_enum()
        {
            var r = Enum.GetIntegralValues<ByteDummyEnum>().ToArray();

            CollectionAssert.AreEqual(new decimal[] { 22, 33, 44 }, r);
        }
    }

	public enum DummyEnum
	{
		A = 0,
		B = 1,
		C = 2
	}

    public enum ByteDummyEnum : byte
    {
        A = 22,
        B = 33,
        C = 44
    }
}
