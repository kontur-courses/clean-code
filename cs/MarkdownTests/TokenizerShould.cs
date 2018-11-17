using Markdown.Tokenizing;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class TokenizerShould
    {
        private Tokenizer markdownTokenizer;

        [SetUp]
        public void SetUp()
        {
            markdownTokenizer = new Tokenizer(new Markdown.Languages.MarkdownLanguage());
        }

        [Test]
        public void DoSomething_WhenSomething()
        {
            
        }
    }
}
