using System.Collections.Generic;

namespace MarkdownProcessing.Tokens
{
    public class ComplicatedToken : Token
    {
        public List<Token> ChildTokens = new List<Token>();

        public ComplicatedToken(TokenType type) => Type = type;
    }
}