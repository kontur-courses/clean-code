using System.Collections.Generic;

namespace Markdown
{
    public class StrongTextToken : TextToken, ITagToken
    {
        public StrongTextToken(string text, List<IToken> subTokens)
            : base(text.Length + 4, TokenType.Strong, text, false, subTokens)
        {
            TextWithTags = $"__{text}__";
        }

        public string TextWithTags { get; }
    }
}