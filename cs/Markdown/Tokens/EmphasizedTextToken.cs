using System.Collections.Generic;

namespace Markdown
{
    public class EmphasizedTextToken : TextToken, ITagToken
    {
        public EmphasizedTextToken(string text, List<IToken> subTokens = null) 
            : base(text.Length + 2, TokenType.Emphasized, text, false, subTokens)
        {
            TextWithTags = $"_{text}_";
        }

        public string TextWithTags { get; }
    }
}