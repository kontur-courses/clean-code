using FluentAssertions;
using Markdown.Tags;
using Markdown.TokenParser.ConcreteParser;
using Markdown.TokenParser.Interfaces;
using MarkdownTests.TestData;

namespace MarkdownTests
{
    public class LineParserTests
    {
        private ITokenLineParser parser = new LineParser();

        [Test]
        public void ParseLine_ThrowArgumentNullException_WhenArgumentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => parser.ParseLine(null), "String argument text must be not null");
        }

        [Test]
        public void ParseLine_ShouldBeEmpty_WhenArgumentStringIsEmpty()
        {
            var parsedLine = parser.ParseLine(String.Empty);
            parsedLine.Line.Should().BeEmpty();
            parsedLine.Tags.Should().BeEmpty();
        }

        [TestCaseSource(typeof(LineParserData), nameof(LineParserData.WordsOnlyLines))]
        public void ParseLine_ShoudBeCorrect_WhenLineWithWordsOnly(string inLine, string expectedLine, List<ITag> tags)
        {
            var parsedLines = parser.ParseLine(inLine);

            parsedLines.Line.Should().BeEquivalentTo(expectedLine);
            parsedLines.Tags.Should().BeEquivalentTo(tags);
        }

        [TestCaseSource(typeof(LineParserData), nameof(LineParserData.LineWithHeader))]
        public void ParseLine_ShoudBeCorrect_WhenLineWithHeaderTags(string inLine, string expectedLine, List<ITag> tags)
        {
            var parsedLines = parser.ParseLine(inLine);

            parsedLines.Line.Should().BeEquivalentTo(expectedLine);
            parsedLines.Tags.Should().BeEquivalentTo(tags);
        }

        [TestCaseSource(typeof(LineParserData), nameof(LineParserData.LinesWithBulletedList))]
        public void ParseLine_ShoudBeCorrect_WhenLinesWithBulletedList(string inLine, string expectedLine, List<ITag> tags)
        {
            var parsedLines = parser.ParseLine(inLine);

            parsedLines.Line.Should().BeEquivalentTo(expectedLine);
            parsedLines.Tags.Should().BeEquivalentTo(tags);
        }

        [TestCaseSource(typeof(LineParserData), nameof(LineParserData.LineWithItalic))]
        public void ParseLine_ShoudBeCorrect_WhenLineWithItalicTags(string inLine, string expectedLine, List<ITag> tags)
        {
            var parsedLines = parser.ParseLine(inLine);

            parsedLines.Line.Should().BeEquivalentTo(expectedLine);
            parsedLines.Tags.Should().BeEquivalentTo(tags);
        }

        [TestCaseSource(typeof(LineParserData), nameof(LineParserData.LineWithBold))]
        public void ParseLine_ShoudBeCorrect_WhenLineWithBoldTags(string inLine, string expectedLine, List<ITag> tags)
        {
            var parsedLines = parser.ParseLine(inLine);

            parsedLines.Line.Should().BeEquivalentTo(expectedLine);
            parsedLines.Tags.Should().BeEquivalentTo(tags);
        }

        [TestCaseSource(typeof(LineParserData), nameof(LineParserData.MultiTagsLine))]
        public void ParseLine_ShoudBeCorrect_WhenLineWithMultiTags(string inLine, string expectedLine, List<ITag> tags)
        {
            var parsedLines = parser.ParseLine(inLine);

            parsedLines.Line.Should().BeEquivalentTo(expectedLine);
            parsedLines.Tags.Should().BeEquivalentTo(tags);
        }
    }
}
