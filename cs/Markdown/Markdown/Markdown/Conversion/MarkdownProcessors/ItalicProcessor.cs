
using System.Linq;

namespace Markdown.MarkdownProcessors
{
    public class ItalicProcessor : IMarkProcessor
    {
        public TokenMd FormatToken(TokenMd token)
        {
            var isInnerStrong = false;
            var startMark = token.Mark.FormattedMarkSymbols.startFormattedMark;
            var endMark = token.Mark.FormattedMarkSymbols.endFormattedMark;

            if (token.InnerTokens != null && token.InnerTokens.Count > 0)
            {
                token.InnerTokens
                    .Where(innerToken => innerToken.Mark != null && innerToken.Mark is StrongMark)
                    .Select(t =>t.Mark = new EmptyMark());
            }

            var formattedTokenText = startMark + token.FormattedText + endMark;

            var resultToken = new TokenMd(formattedTokenText, token.Mark)
            {
                FormattedText = formattedTokenText, 
                InnerTokens = token.InnerTokens
            };
            return resultToken;
        }
    }
}