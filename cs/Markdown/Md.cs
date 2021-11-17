namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            Tokenizer tokenizer = new Tokenizer();
            return  new HtmlConvector().Convert(tokenizer.Parse(text));
        }
    }
}
