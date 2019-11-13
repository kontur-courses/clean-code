using Markdown.Tests;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            var tokenizer = new MdTokenizer();
            var tokens = tokenizer.Tokenize(text);
            var renderer = new HTMLRenderer();
            return renderer.Render(tokens);
        }
    }
}
