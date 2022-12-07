namespace Markdown
{
    public class Token : IToken
    {
        public TokenType Type { get; }
        public TagState TagState { get; }
        public string Content { get; }
        
        public Token(TokenType type, string content)
        {
            Type = type;
            Content = content;
        }

        public Token(TokenType type, string content, TagState tagState)
        {
            Type = type;
            Content = content;
            TagState = tagState;
        }

        public object Clone()
        {
            return new Token(Type, new string(Content), TagState);
        }
    }
}
