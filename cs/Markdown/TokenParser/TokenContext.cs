using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown.TokenParser
{
    public class TokenContext
    {
        public readonly Token Token;
        public readonly List<TokenNode> Children = new();
        public readonly bool IsInMiddleOfWord;

        public TokenContext(Token token, bool isInMiddleOfWord)
        {
            IsInMiddleOfWord = isInMiddleOfWord;
            Token = token;
        }

        public bool ContainsWhiteSpace => Children.Any(x => x.Token.Value.Contains(" "));

        public string ToText()
        {
            if (Children.Count == 0)
                return Token.Value;
            return Token.Value + StringUtils.Join(Children.Select(x => x.ToText()));
        }
    }
}