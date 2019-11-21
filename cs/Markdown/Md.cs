namespace Markdown
{
    public class Md
    {
        public string Render(string paragraph)
        {
            var tokenizer = new MdTokenizer();
            var tokens = tokenizer.Tokenize(paragraph);
            var renderer = new HTMLRenderer();
            return renderer.Render(tokens);
        }
    }
}