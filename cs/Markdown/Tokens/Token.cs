namespace Markdown
{
    public class Token
    {
        public readonly int Length;
        public TokenType Type { get; }
        public string Content { get; }

        public Token(TokenType type, string content, int length)
        {
            Type = type;
            Content = content;
            Length = length;
        }

        public Token(TokenType type, string content)
        {
            Type = type;
            Content = content;
            Length = content.Length;
        }

        public Token(TokenType type)
        {
            Type = type;
            Content = TokenTypesTranslator.GetStringFromTokenType(type);
            Length = Content.Length;
        }

        public char GetLastContentChar()
        {
            return Content[Content.Length - 1];
        }

        public char GetFirstContentChar()
        {
            return Content[0];
        }

        public Token GetTextToken()
        {
            return new Token(TokenType.Text, Content, Length);
        }

        public override string ToString()
        {
            return Content;
        }
    }
}