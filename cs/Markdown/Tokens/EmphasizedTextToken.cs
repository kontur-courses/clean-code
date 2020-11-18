using System.Collections.Generic;

namespace Markdown
{
    public class EmphasizedTextToken : TextToken, ITagToken
    {
        public EmphasizedTextToken(string text, List<IToken> subTokens = null)
            : base(text.Length, TokenType.Emphasized, text, false, subTokens)
        {
            TextWithoutTags = text[1..(text.Length - 1)];
            var a = TextWithoutTags;
        }

        public string TextWithoutTags { get; }
    }
}