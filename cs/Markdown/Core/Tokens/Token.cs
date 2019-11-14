namespace Markdown.Core.Tokens
{
    public class Token : IToken
    {        
        public int Position { get; }
        public int Length { get; }
        public string Value { get; }
        public TokenType TokenType { get; set; }

        protected Token(int position, string value, TokenType tokenType)
        {
            Position = position;
            Value = value;
            Length = value.Length;
            TokenType = tokenType;
        }
    }
}