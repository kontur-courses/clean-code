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
            new TestCaseData("# Test").Returns("<h1> Test</h1>").SetName("PassHeadingSymbol"),
            new TestCaseData("# __Test__").Returns("<h1> <strong>Test</strong></h1>").SetName("PassHeadingSymbolWithBoldInside"),
            new TestCaseData("# __Te_s_t__").Returns("<h1> <strong>Te<em>s</em>t</strong></h1>").SetName("PassHeadingSymbolWithBoldAndItalicInside"),
            new TestCaseData("\\#Вот это").Returns("#Вот это").SetName("EscapedHeadingSymbol"),
        };

        [Test]
        [TestCaseSource(nameof(headingTestCases))]
        public string WhenPassArguments_ShouldConvertToCorrectHtml(string input) =>
            sut.Render(input);
    }
}