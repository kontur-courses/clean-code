using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class ParserShould
    {
        private ITokenParser parser;
        
        [SetUp]
        public void SetUp()
        {
            parser = TokenParserConfigurator.CreateTokenParser()
                .AddToken(new Token("-"))
                .AddToken(new Token("--"))
                .AddToken(new Token("__"))
                .AddToken(new Token("_<"))
                .Configure();
        }

        [TestCase("-a", true, false)]
        [TestCase("a-", false, true)]
        [TestCase("-", false, false)]
        [TestCase("a-a", true, true)]
        [TestCase("a -a", true, false)]
        [TestCase("a- a", false, true)]
        [TestCase("a-- a", false, true)]
        [TestCase("a --a", true, false)]
        public void FindAllTokens_Validation_Test(string text, bool expectedValidToStart, bool expectedValidToClose)
        {
            var actualToken = parser
                .FindAllTokens(text)
                .First()
                .Value;

            var actualValidToStart = actualToken.OpenValid;
            var actualValidToClose = actualToken.CloseValid;

            actualValidToStart.Should().Be(expectedValidToStart);
            actualValidToClose.Should().Be(expectedValidToClose);
        }
        
        [TestCase("a-a", 1)]
        [TestCase("a-a-a", 1, 3)]
        [TestCase("-a", 0)]
        [TestCase("a-", 1)]
        [TestCase("a--a", 1)]
        [TestCase("a---a", 1, 3)]
        [TestCase("a----a", 1, 3)]
        [TestCase("a--a--a", 1, 4)]
        [TestCase("a__a-_<a", 1, 4, 5)]
        [TestCase("a__", 1)]
        [TestCase("__a", 0)]
        [TestCase("a_<", 1)]
        [TestCase("_<a", 0)]
        public void FindAllTokens_Location_Test(string text, params int[] expectedIndexes)
        {
            var actualIndexes = parser
                .FindAllTokens(text)
                .Select(x => x.Key)
                .OrderBy(x => x)
                .ToArray();

            actualIndexes
                .Should()
                .Equal(expectedIndexes
                    .OrderBy(x => x)
                    .ToArray()
                );
        }
    }
}