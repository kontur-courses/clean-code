using System.Collections.Generic;

namespace Markdown
{
    public class StrongTextToken : TextToken, ITagToken
    {
        public StrongTextToken(string text, List<IToken> subTokens = null)
            : base(text.Length, TokenType.Strong, text, false, subTokens)
        {
            TextWithoutTags = text[2..(text.Length - 2)];
            var a = TextWithoutTags;
        }

        public string TextWithoutTags { get; }
    }
}