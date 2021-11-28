namespace Markdown.Engine.Tokens
{
    public class Whitespace : IToken
    {
        public TokenType TokenType => TokenType.WhiteSpace;
        public string Value => " ";
        public bool CanParse(string symbol) => symbol == " ";
        public IToken Create(string[] text, int index) => this;
    }
}