using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    class MarkdownTests
    {
        [TestCase("_d_", ExpectedResult = "<p><em>d</em></p>", TestName = "Works with single underscore")]
        [TestCase("__d__", ExpectedResult = "<p><strong>d</strong></p>", TestName = "Works with double underscore")]
        [TestCase("# d", ExpectedResult = "<h1>d</h1>", TestName = "Works with lattice")]
        [TestCase("## d", ExpectedResult = "<h2>d</h2>", TestName = "Works with double lattice")]
        [TestCase("### d", ExpectedResult = "<h3>d</h3>", TestName = "Works with triple lattice")]
        [TestCase("#### d", ExpectedResult = "<h4>d</h4>", TestName = "Works with quadruple lattice")]
        [TestCase("* d", ExpectedResult = "<ul><li>d</li></ul>", TestName = "Works with star")]
        [TestCase("* d\n* d", ExpectedResult = "<ul><li>d</li><li>d</li></ul>", TestName = "Works with double star")]
        [TestCase("__d _d__ d_", ExpectedResult = "<p><strong>d _d</strong> d_</p>", TestName = "Works after tokens crossing")]
        public string TestInlineTokensValidation(string text)
        {
            return new Markdown.Markdown().Render(text);
        }
    }
}
