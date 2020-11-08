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
    }
}