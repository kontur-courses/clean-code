using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTest
{
    [TestFixture]
    public class MdTests
    {
        private Md sut;

        [SetUp]
        public void Init()
        {
            sut = new Md();
        }

        private static TestCaseData[] mdTestsCases =
        {
            new TestCaseData("__двойного _одинарное_ тоже__", "<strong>двойного <em>одинарное</em> тоже</strong>").SetName("ItalicTagInsideBoldTag"),
            new TestCaseData("__разные выделения_", "__разные выделения_").SetName("PassUnpairedSymbols"),
            new TestCaseData("# _Test_", "<h1> <em>Test</em></h1>").SetName("italicInsideHeading"),
            new TestCaseData("__abc__ _abc_ _abc_ __abc__", "<strong>abc</strong> <em>abc</em> <em>abc</em> <strong>abc</strong>").SetName("MultipleTagsInARow")
        };

        [Test]
        [TestCaseSource(nameof(mdTestsCases))]
        public void WhenItalicTagInsideBoldTag_ShouldReturnTwoTags(string input, string expected)
        {
            var actual = sut.Render(input);

            actual.Should().Be(expected);
        }
    }
}
