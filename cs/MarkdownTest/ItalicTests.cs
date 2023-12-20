using Markdown;
using NUnit.Framework;

namespace MarkdownTest
{
    [TestFixture]
    public class ItalicTests
    {
        private Md? sut;

        [SetUp]
        public void Init()
        {
            sut = new Md();
        }

        private static TestCaseData[] italicTestCases =
        {
            new TestCaseData("_Test_").Returns("<em>Test</em>").SetName("PassItalicSymbol"),
            new TestCaseData("_нач_але").Returns("<em>нач</em>але").SetName("ItalicsInPartOfWord"),
            new TestCaseData("_одинарного __двойное__ не_").Returns( "<em>одинарного __двойное__ не</em>").SetName("BoldInsideItalicsTag"),
            new TestCaseData("\\_Вот это\\_").Returns("_Вот это_").SetName("EscapedItalicSymbol"),
            new TestCaseData("Здесь сим\\волы \\должны остаться.\\").Returns("Здесь сим\\волы \\должны остаться.\\").SetName("EscapeSymbolThatDoesNotEscapeAnything"),
            new TestCaseData("\\\\_вот это будет выделено тегом_").Returns("<em>вот это будет выделено тегом</em>").SetName("EscapedEscapeSymbol"),
            new TestCaseData("раз_ных сло_вах").Returns("раз_ных сло_вах").SetName("ItalicTagOnHalfOfTwoDifferentWords"),
            new TestCaseData("__").Returns("__").SetName("EmptyStringInItalicTag"),
        };

        [TestCaseSource(nameof(italicTestCases))]
        public string WhenPassArguments_ShouldConvertToCorrectHtml(string input) =>
            sut.Render(input);
    }
}