using System.Text;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
    public class Md_Should
    {
        private Md reader = new Md();
        [TestCase("aghtr _something_", "aghtr <em>something</em>", TestName = "SimpleUnderscore")]
        [TestCase("aghtr _12_5", "aghtr _12_5", TestName = "NotTranslateWithNumbers")]
        [TestCase("_text_", "<em>text</em>", TestName = "TheWholeText")]
        [TestCase("some _ text_", "some _ text_", TestName = "NotTranslateIfSpaceAfter")]
        [TestCase("some _text _plus one_", "some <em>text _plus one</em>", TestName = "NotTranslateIfSpaceBefore")]
        [TestCase("some \\_text_", "some _text_", TestName = "IgnoreScreeningUnderscores")]
        [TestCase("some _text", "some _text", TestName = "WithoutClosingUnderscore")]
        [TestCase("one _some _inner text_ text_ text", "one <em>some _inner text</em> text_ text", TestName = "DoNotTranslateInnerUnderscores")]
        public void Solve(string mdText, string htmlText)
        {
            var result = reader.Render(mdText);
            result.Should().Be(htmlText);
        }
    }
}