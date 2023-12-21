using Markdown;
using NUnit.Framework;

namespace MarkdownTest
{
    [TestFixture]
    public class MdTests
    {
        private Md? sut;

        [SetUp]
        public void Init()
        {
            sut = new Md();
        }

        private static TestCaseData[] mdTestCases =
        {
            new TestCaseData("__двойного _одинарное_ тоже__").Returns("<strong>двойного <em>одинарное</em> тоже</strong>").SetName("ItalicTagInsideBoldTag"),
            new TestCaseData("__разные выделения_").Returns("__разные выделения_").SetName("PassUnpairedSymbols"),
            new TestCaseData("# _Test_").Returns("<h1> <em>Test</em></h1>").SetName("italicInsideHeading"),
            new TestCaseData("__abc__ _abc_ _abc_ __abc__").Returns("<strong>abc</strong> <em>abc</em> <em>abc</em> <strong>abc</strong>").SetName("MultipleTagsInARow"),
            new TestCaseData("#abcabc \n#abcabc").Returns("<h1>abcabc</h1> \n<h1>abcabc</h1>").SetName("TwoHeadingsWithNewLineSymbol"),
            new TestCaseData("цифрами_12_3").Returns("цифрами_12_3").SetName("digitsInsideWord"),
            new TestCaseData("цифрами__12__3").Returns("цифрами__12__3").SetName("digitsInsideWord2"),
            new TestCaseData("__abc__ #abc").Returns("<strong>abc</strong> #abc").SetName("HeadingInPartOfWord"),
            new TestCaseData("__abc__ \n#abc").Returns("<strong>abc</strong> \n<h1>abc</h1>").SetName("HeadingWithNewLineSymbol"),
            new TestCaseData("__text").Returns("__text").SetName("ClosingTagIsMissing"),
            new TestCaseData("text_").Returns("text_").SetName("OpenTagIsMissing"),
            new TestCaseData("start_ stop_").Returns("start_ stop_").SetName("SpaceSymbolAfterOpeningTag"),
            new TestCaseData("start_stop _").Returns("start_stop _").SetName("SpaceSymbolBeforeClosingTag"),
            new TestCaseData("__пересечения _двойных__ и одинарных_").Returns("__пересечения _двойных__ и одинарных_").SetName("IntersectionBoldAndItalicTag"),
            new TestCaseData("_пересечения __двойных_ и одинарных__").Returns("_пересечения __двойных_ и одинарных__").SetName("IntersectionItalicAndBoldTag"),
            new TestCaseData("\\\\\\").Returns("\\\\\\").SetName("StringOfSlashes")
        };

        [TestCaseSource(nameof(mdTestCases))]
        public string WhenItalicTagInsideBoldTag_ShouldReturnTwoTags(string input) => 
            sut.Render(input);
    }
}