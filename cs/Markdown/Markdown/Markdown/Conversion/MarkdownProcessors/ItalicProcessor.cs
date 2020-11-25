
namespace Markdown.MarkdownProcessors
{
    public class ItalicProcessor : IMarkProcessor
    {
        public TokenMd FormatToken(TokenMd token)
        {
            var isInnerStrong = false;
            var startMark = token.Mark.FormattedMarkSymbols.startFormattedMark;
            var endMark = token.Mark.FormattedMarkSymbols.endFormattedMark;

            if(token.InnerTokens!=null && token.InnerTokens.Count>0)
                foreach (var innerToken in token.InnerTokens)
                {
                    if (innerToken.Mark != null && innerToken.Mark.GetType() == typeof(StrongMark))
                    {
                        isInnerStrong = true;
                        innerToken.Mark = null;
                    }
                }

            if (isInnerStrong)
            {
                var resultWordToken = new TokenMd(token.Token, token.Mark);
                resultWordToken.FormattedText = token.Token;
                resultWordToken.InnerTokens = token.InnerTokens;
            }
            var formattedTokenText = startMark + token.FormattedText + endMark;
            
            var resultToken = new TokenMd(formattedTokenText, token.Mark);
            resultToken.FormattedText = formattedTokenText;
            resultToken.InnerTokens = token.InnerTokens;
            return resultToken;
        }
    }
}