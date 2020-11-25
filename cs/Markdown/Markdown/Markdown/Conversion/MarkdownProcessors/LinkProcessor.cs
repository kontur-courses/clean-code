﻿
namespace Markdown.Conversion.MarkdownProcessors
{
    public class LinkProcessor : IMarkProcessor
    {
        public TokenMd FormatToken(TokenMd token)
        {
            var resultToken = token;
            var mark = token.Mark as LinkMark;

            var tokenText = $"<a href=\"{mark.Link}\">{resultToken.FormattedText}</a>";
            resultToken.Token = tokenText;
            resultToken.FormattedText = tokenText;
            
            return resultToken;
        }
    }
}