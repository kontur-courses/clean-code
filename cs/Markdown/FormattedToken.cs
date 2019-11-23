using System.Collections.Generic;
using Markdown.BasicTextTokenizer;

namespace Markdown
{
    public class FormattedToken
    {
        public FormattedTokenType Type { get; }
        public List<FormattedToken> Content { get; }
        public int StartIndex { get; }
        public int EndIndex { get; }
        public int Length => EndIndex - StartIndex + 1;

        public FormattedToken(List<FormattedToken> content, FormattedTokenType type, int startIndex, int endIndex)
        {
            Content = content;
            Type = type;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        public static FormattedToken GetTextToken(Token token)
        {
            return new FormattedToken(null, FormattedTokenType.Raw,
                token.Position, token.Length + token.Position - 1);
        }
    }
}
