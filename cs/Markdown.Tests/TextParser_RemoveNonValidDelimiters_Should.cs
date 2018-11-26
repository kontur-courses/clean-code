using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class TextParser_RemoveNonValidDelimiters_Should
    {
        [Category("UnderscoreRule")]
        [TestCase("1_2", TestName = "surrounded by digits")]
        [TestCase("a_b", TestName = "surrounded by letters")]
        [TestCase("a_2", TestName = "surrounded by letters and digits")]
        [TestCase("a__2", TestName = "surrounded by letters and digits and there are two underscores")]
        [TestCase("a___2", TestName = "surrounded by letters and digits and there are three underscores")]
        public void RemoveUnderscore_When(string text)
        {
            TextParser.For(text)
                      .GetDelimiterPositions()
                      .RemoveEscapedDelimiters()
                      .RemoveNonValidDelimiters()
                      .Delimiters.Should()
                      .BeEmpty();
        }
    }
}
