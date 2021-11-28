namespace Markdown.TokenCreator.Tokens
{
    public class BracketClose : IToken
    {
        public TokenType TokenType => TokenType.BracketClose;
        public string Value => ")";
        public bool CanParse(string symbol) => symbol == Value;

        public IToken Create(string[] text, int index) => new BracketClose();
    }
}