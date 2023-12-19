using FluentAssertions;
using NUnit.Framework;
using Markdown;

namespace MarkdownTest
{
    [TestFixture]
    public class HeadingTests
    {
        private Md sut;

        [SetUp]
        public void Init()
        {
            sut = new Md();
        }

        private static TestCaseData[] headingTestCases =
        {
            new TestCaseData("# Test", "<h1> Test</h1>").SetName("PassHeadingSymbol"),
            new TestCaseData("# __Test__", "<h1> <strong>Test</strong></h1>").SetName("PassHeadingSymbolWithBoldInside"),
            new TestCaseData("# __Te_s_t__", "<h1> <strong>Te<em>s</em>t</strong></h1>").SetName("PassHeadingSymbolWithBoldAndItalicInside"),
            new TestCaseData("\\#Вот это", "#Вот это").SetName("EscapedHeadingSymbol"),
        };

        [Test]
        [TestCaseSource(nameof(headingTestCases))]
        public void WhenPassArguments_ShouldConvertToCorrectHtml(string input, string expected)
        {
            var actual = sut.Render(input);

            actual.Should().Be(expected);
        }
    }
}
