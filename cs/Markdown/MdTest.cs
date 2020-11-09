using NUnit.Framework;

namespace Markdown
{
    public class Tests
    {
        [TestCase("", ExpectedResult = "")]
        [TestCase(" ", ExpectedResult = " ")]
        [TestCase("a", ExpectedResult = "a")]
        [TestCase("some text", ExpectedResult = "some text")]
        public string Test_TextWithoutTags(string text)
        {
            return Md.Render(text);
        }

        [TestCase("_some text_", ExpectedResult = @"<em>some text<\em>")]
        [TestCase("a _some text_ 89", ExpectedResult = @"a <em>some text<\em> 89")]
        [TestCase("_some text_ a _some text 2_", ExpectedResult = @"<em>some text<\em> a <em>some text 2<\em>")]
        public string Test_TextWithTagEm(string text) 
        {
            return Md.Render(text);
        }
    }
}