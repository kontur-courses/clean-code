using System.Collections.Generic;

namespace Markdown
{
    public class HeaderTextToken : TextToken, ITagToken
    {
        public HeaderTextToken(string text, List<IToken> subTokens = null) 
            : base(text.Length + 1, TokenType.Header, text, false, subTokens)
        {
            TextWithTags = $"#{text}";
        }

        public string TextWithTags { get; }
    }
}