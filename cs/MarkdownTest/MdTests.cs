using System.Diagnostics;
using FluentAssertions;
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
            new TestCaseData("__двойного _одинарное_ тоже__", "<strong>двойного <em>одинарное</em> тоже</strong>").SetName("ItalicTagInsideBoldTag"),
            new TestCaseData("__разные выделения_", "__разные выделения_").SetName("PassUnpairedSymbols"),
            new TestCaseData("# _Test_", "<h1> <em>Test</em></h1>").SetName("italicInsideHeading"),
            new TestCaseData("__abc__ _abc_ _abc_ __abc__", "<strong>abc</strong> <em>abc</em> <em>abc</em> <strong>abc</strong>").SetName("MultipleTagsInARow"),
            new TestCaseData("#abcabc \n#abcabc", "<h1>abcabc</h1> \n<h1>abcabc</h1>").SetName("TwoHeadingsWithNewLineSymbol"),
            new TestCaseData("цифрами_12_3", "цифрами_12_3").SetName("digitsInsideWord"),
            new TestCaseData("цифрами__12__3", "цифрами__12__3").SetName("digitsInsideWord2"),
            new TestCaseData("__abc__ #abc", "<strong>abc</strong> #abc").SetName("HeadingInPartOfWord"),
            new TestCaseData("__abc__ \n#abc","<strong>abc</strong> \n<h1>abc</h1>").SetName("HeadingWithNewLineSymbol"),
            new TestCaseData("__text","__text").SetName("ClosingTagIsMissing"),
            new TestCaseData("text_","text_").SetName("OpenTagIsMissing"),
            new TestCaseData("start_ stop_","start_ stop_").SetName("SpaceSymbolAfterOpeningTag"),
            new TestCaseData("start_stop _","start_stop _").SetName("SpaceSymbolBeforeClosingTag"),
            new TestCaseData("__пересечения _двойных__ и одинарных_", "__пересечения _двойных__ и одинарных_").SetName("gggg"),
            new TestCaseData("_пересечения __двойных_ и одинарных__", "_пересечения __двойных_ и одинарных__").SetName("giggg"),
        };

        [TestCaseSource(nameof(mdTestCases))]
        public void WhenItalicTagInsideBoldTag_ShouldReturnTwoTags(string input, string expected)
        {
            var actual = sut?.Render(input);

            actual.Should().Be(expected);
        }

        [Test]
        public void TestToCheckComplexity()
        {
            for (var i = 0; i < 10; i++)
                sut.Render("__двойного _одинарное_ тоже__");

            var string1 = "";
            var string2 = "";

            for (var i = 0; i < 10; i++)
                string1 += "__двойного _одинарное_ тоже__";

            for (var i = 0; i < 100; i++)
                string2 += "__двойного _одинарное_ тоже__";

            var sw = Stopwatch.StartNew();
            for (var i = 0; i < 5; i++)
                 sut.Render(string1);
            sw.Stop();

            var timeSpan1 = sw.Elapsed / 5;

            var sw2 = Stopwatch.StartNew();
            for (var i = 0; i < 5; i++)
                 sut.Render(string2);
            sw2.Stop();

            var timeSpan2 = sw2.Elapsed / 5;
            
            var r1 = timeSpan1 / string1.Length;
            var r2 = timeSpan2 / string2.Length;

            Assert.AreEqual(r1, r2);
        }
    }
}