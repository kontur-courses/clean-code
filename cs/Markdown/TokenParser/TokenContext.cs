using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.TokenParser
{
    public class TokenContext
    {
        public readonly Token Token;
        public readonly List<TokenNode> Children = new();
        public readonly bool IsSplitWord;

        public TokenContext(Token token, bool isSplitWord)
        {
            IsSplitWord = isSplitWord;
            Token = token;
        }

        public string ToText()
        {
            if (Children.Count == 0)
                return Token.Value;
            return Token.Value + string.Join("", Children.Select(x => x.ToText()));
        }
    }
}