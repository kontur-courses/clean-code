using System.Collections.Generic;
using MarkDown;
using NUnit.Framework;

namespace MarkDown_Tests
{
    [TestFixture]
    public class StringExtensions_Should
    {
        private List<string> specialCharacters;
        [SetUp]
        public void SetUp()
        {
            specialCharacters = new List<string>(){"_", "__", @"\"};
        }

        [TestCase(@"\a", ExpectedResult = @"\a", TestName = "when escaping not special character")]
        [TestCase(@"\_a\_", ExpectedResult = @"_a_", TestName = "when escaping single underscore")]
        [TestCase(@"\__a\__", ExpectedResult = @"__a__", TestName = "when escaping double underscore")]
        [TestCase(@"\__a\__", ExpectedResult = @"__a__", TestName = "when escaping double underscore")]
        [TestCase(@"\\_a\\_", ExpectedResult = @"_a_", TestName = "when escaping escaped single underscore")]
        [TestCase(@"a\\a", ExpectedResult = @"a\a", TestName = "when escaping escape character")]
        public string Escape_CharactersCorrectly(string text) => text.Escape(specialCharacters);
    }
}
