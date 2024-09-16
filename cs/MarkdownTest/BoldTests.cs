using NUnit.Framework;
using Markdown;

namespace MarkdownTest
{
    [TestFixture]
    public class BoldTests
    {
        private Md? sut;

        [SetUp]
        public void Init()
        {
            sut = new Md();
        }

        private static TestCaseData[] boldTestCases =
        {
            new TestCaseData("__Test__").Returns("<strong>Test</strong>").SetName("PassItalicSymbol"),
            new TestCaseData("__два _один_ может__").Returns("<strong>два <em>один</em> может</strong>").SetName("BoldInsideItalicsTag"),
            new TestCaseData("\\__Вот это\\__").Returns("__Вот это__").SetName("EscapedBoldSymbol"),
            new TestCaseData("____").Returns("____").SetName("EmptyStringInBoldTag"),
            new TestCaseData("раз__ных сло__вах").Returns("раз__ных сло__вах").SetName("BoldTagOnHalfOfTwoDifferentWords")
        };

        [TestCaseSource(nameof(boldTestCases))]
        public string WhenPassBoldSymbol_ShouldConvertToStrongHtml(string input) =>
            sut.Render(input);

    }
}