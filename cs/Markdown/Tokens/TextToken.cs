using System.Collections.Generic;

namespace Markdown.Tokens
{
    public class TextToken : IToken
    {
        protected TextToken(TokenType type, string text)
        {
            Length = text.Length;
            Type = type;
            Text = text;
        }


        public int Length { get; }
        public TokenType Type { get; }
        public string Text { get; protected set; }
        public List<IToken> SubTokens { get; set; }
    }
}