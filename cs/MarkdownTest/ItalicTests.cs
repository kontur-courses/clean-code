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
        };

        [Test]
        [TestCaseSource(nameof(italicTestCases))]
        public void WhenPassArguments_ShouldConvertToCorrectHtml(string input, string expected)
        {
            var actual = sut.Render(input);

            actual.Should().Be(expected);
        }
    }
}
