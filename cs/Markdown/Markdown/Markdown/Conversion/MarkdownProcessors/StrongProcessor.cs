namespace Markdown.Conversion.MarkdownProcessors
{
    public class StrongProcessor : IMarkProcessor
    {
        public TokenMd FormatToken(TokenMd token)
        {
            var startMark = token.Mark.FormattedMarkSymbols.startFormattedMark;
            var endMark = token.Mark.FormattedMarkSymbols.endFormattedMark;

            var formattedTokenText = "";
            if (token.External?.Mark == null)
                formattedTokenText = startMark + token.FormattedText + endMark;
            
            if(token.External?.Mark != null && token.External.Mark.GetType()!=typeof(ItalicMark))
                formattedTokenText = startMark + token.FormattedText + endMark;
            
            var resultToken = new TokenMd(formattedTokenText, token.Mark);
            resultToken.FormattedText = formattedTokenText;
            resultToken.InnerTokens = token.InnerTokens;
            
            return resultToken;
        }
    }
}