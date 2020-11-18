using System.Collections.Generic;

namespace Markdown
{
    public class HeaderTextToken : TextToken, ITagToken
    {
        public HeaderTextToken(string text, List<IToken> subTokens = null) 
            : base(text.Length, TokenType.Header, text, false, subTokens)
        {
            TextWithoutTags = text[1..];
            var a = TextWithoutTags;
        }

        public string TextWithoutTags { get; }
    }
}