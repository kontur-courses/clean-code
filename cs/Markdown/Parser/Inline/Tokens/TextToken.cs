namespace Markdown.Parser.Inline.Tokens
{
    public class TextToken : Token
    {
        public string Text { get; }
        public TextToken(string text, int position) : base(position) => Text = text;
    }
}