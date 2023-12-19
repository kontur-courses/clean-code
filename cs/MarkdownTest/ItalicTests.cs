using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTest
{
    [TestFixture]
    public class ItalicTests
    {
        private Md sut;

        [SetUp]
        public void Init()
        {
            sut = new Md();
        }

        private static TestCaseData[] italicTestCases =
        {
            new TestCaseData("_Test_", "<em>Test</em>").SetName("PassItalicSymbol"),
            new TestCaseData("_нач_але", "<em>нач</em>але").SetName("ItalicsInPartOfWord"),
            new TestCaseData("_одинарного __двойное__ не_", "<em>одинарного __двойное__ не</em>").SetName("BoldInsideItalicsTag"),
            new TestCaseData("\\_Вот это\\_", "_Вот это_").SetName("EscapedItalicSymbol"),
            new TestCaseData("Здесь сим\\волы \\должны остаться.\\", "Здесь сим\\волы \\должны остаться.\\").SetName("EscapeSymbolThatDoesNotEscapeAnything"),
            new TestCaseData("\\\\_вот это будет выделено тегом_", "<em>вот это будет выделено тегом</em>").SetName("EscapedEscapeSymbol")
        };

        [TestCaseSource(nameof(italicTestCases))]
        public void WhenPassArguments_ShouldConvertToCorrectHtml(string input, string expected)
        {
            var actual = sut.Render(input);

            actual.Should().Be(expected);
        }
    }
}
