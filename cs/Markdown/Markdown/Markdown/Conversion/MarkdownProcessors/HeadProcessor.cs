using System.Collections.Generic;
using System.Text;

namespace Markdown.Conversion.MarkdownProcessors
{
    public class HeadProcessor : IMarkProcessor
    {
        public TokenMd FormatToken(TokenMd token)
        {
            var startMark = token.Mark.FormattedMarkSymbols.startFormattedMark;
            var endMark = token.Mark.FormattedMarkSymbols.endFormattedMark;

            var formattedTokenText = startMark + token.FormattedText + endMark;

            var resultToken = new TokenMd(formattedTokenText, token.Mark)
            {
                FormattedText = formattedTokenText, 
                InnerTokens = token.InnerTokens,
            };
            return resultToken;
        }
    }
}