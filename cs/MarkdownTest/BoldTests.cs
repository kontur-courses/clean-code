using NUnit.Framework;
using Markdown;
using FluentAssertions;

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
            new TestCaseData("__Test__", "<strong>Test</strong>").SetName("PassItalicSymbol"),
            new TestCaseData("__два _один_ может__", "<strong>два <em>один</em> может</strong>").SetName("BoldInsideItalicsTag"),
            new TestCaseData("\\__Вот это\\__", "__Вот это__").SetName("EscapedBoldSymbol"),
            new TestCaseData("____", "____").SetName("EmptyStringInBoldTag"),
            new TestCaseData("раз__ных сло__вах", "раз__ных сло__вах").SetName("BoldTagOnHalfOfTwoDifferentWords")
        };

        [TestCaseSource(nameof(boldTestCases))]
        public void WhenPassBoldSymbol_ShouldConvertToStrongHtml(string input, string expected)
        {
            var actual = sut?.Render(input);

            actual.Should().Be(expected);
        }
    }
}