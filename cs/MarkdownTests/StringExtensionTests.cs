using Markdown.StringExtension;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class StringExtensionTests
    {
        
        [TestCase("simple text", 1, "impl", ExpectedResult = false, TestName = "when position positive and less then length text")]
        [TestCase("simple text", -1, "impl", ExpectedResult = false, TestName = "when position negative")]
        [TestCase("text", 1, "text", ExpectedResult = false, TestName = "when substring length more then text length")]
        [TestCase("string", 1, "te", ExpectedResult = true, TestName = "when text have not substring")]
        public bool CompareWithSubstring_ShouldCorrectCompare(string text,int position, string secondString)
        {
            return text.CompareWithSubstring(position, secondString);
        }
    }
}