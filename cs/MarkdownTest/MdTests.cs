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

        private static TestCaseData[] mdTestCases =
        {
            new TestCaseData("__двойного _одинарное_ тоже__", "<strong>двойного <em>одинарное</em> тоже</strong>").SetName("ItalicTagInsideBoldTag"),
            new TestCaseData("__разные выделения_", "__разные выделения_").SetName("PassUnpairedSymbols"),
            new TestCaseData("# _Test_", "<h1> <em>Test</em></h1>").SetName("italicInsideHeading"),
            new TestCaseData("__abc__ _abc_ _abc_ __abc__", "<strong>abc</strong> <em>abc</em> <em>abc</em> <strong>abc</strong>").SetName("MultipleTagsInARow"),
            new TestCaseData("#abcabc \n#abcabc", "<h1>abcabc</h1> \n<h1>abcabc</h1>").SetName("Экранированеи"),
            new TestCaseData("цифрами_12_3", "цифрами_12_3").SetName("digitsInsideWord"),
            new TestCaseData("__abc__ #abc", "<strong>abc</strong> #abc").SetName("HeadingInPartOfWord"),
            new TestCaseData("__abc__ \n#abc","<strong>abc</strong> \n<h1>abc</h1>").SetName("blabla")
        };

        [TestCaseSource(nameof(mdTestCases))]
        public void WhenItalicTagInsideBoldTag_ShouldReturnTwoTags(string input, string expected)
        {
            var actual = sut.Render(input);

            actual.Should().Be(expected);
        }
    }
}