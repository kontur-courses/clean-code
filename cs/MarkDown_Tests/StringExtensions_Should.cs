using System.Collections.Generic;
using FluentAssertions;
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

        public void GetCharStates_Correctly(string text) 
        {
            var expected = new List<Character>
            {
                new Character('\\', CharState.NotEscaped),
                new Character('a', CharState.NotEscaped),
                new Character('_', CharState.NotEscaped),
                new Character('a', CharState.NotEscaped),
                new Character('\\', CharState.Ignored),
                new Character('\\', CharState.Escaped),
                new Character('_', CharState.NotEscaped),
                new Character('\\', CharState.Ignored),
                new Character('_', CharState.Escaped)               
            };
            @"\a_a\\_\_".GetCharStates(specialCharacters).Should().BeEquivalentTo(expected);
        }
    }
}
