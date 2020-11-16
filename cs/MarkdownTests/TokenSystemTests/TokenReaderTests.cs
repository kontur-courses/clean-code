using FluentAssertions;
using Markdown.TokenSystem;
using NUnit.Framework;

namespace MarkdownTests.TokenSystemTests
{
    public class TokenReaderTests
    {
        [SetUp]
        public void SetUp()
        {
            sut = new TokenReader("123_some text!", 0);
        }

        private TokenReader sut;
        
        [TestCase("", "123_some text!")] 
        [TestCase("1", "")] 
        [TestCase(" ", "123_some")] 
        [TestCase("xt", "123_some te")] 
        public void ReadUntil_ReturnSequenceUntilCondition(string stopStr, string expected)
        {
            var result = sut.ReadUntil(str => str == stopStr);

            result.Value.Should().Be(expected);
        }
        
        [TestCase("", "123_some text!")] 
        [TestCase("1", "")] 
        [TestCase(" ", "123_some")] 
        [TestCase("xt", "123_some te")] 
        public void ReadWhile_ReturnSequenceUntilCondition(string stopStr, string expected)
        {
            var result = sut.ReadWhile(str => str != stopStr);

            result.Value.Should().Be(expected);
        }
    }
}