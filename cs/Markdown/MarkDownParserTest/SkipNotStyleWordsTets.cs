using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class SkipNotStyleWordsTests
    {
        [Test]
        public void SkipNotStyleWords_ReturnStringLength_OnStringWithoutUnderscopes()
        {
            MarkdownParser.SkipNotStyleWords("asdf", 0).Should().Be(4);
        }

        [Test]
        public void SkipNotStyleWords_ReturnSubStringLength_OnSubstringWithoutUnderscopes()
        {
            MarkdownParser.SkipNotStyleWords("a asdf", 2).Should().Be(4);
        }

        [Test]
        public void SkipNotStyleWords_ReturnZero_WhenStringStartWithUndersope()
        {
            MarkdownParser.SkipNotStyleWords("_asdf_", 0).Should().Be(0);
            
        }
        
    }
}