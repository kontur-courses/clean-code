using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Conversion.MarkdownProcessors;
using Markdown.MarkdownProcessors;

namespace Markdown
{
    public class MarkdownProcessor : IMarkdownProcessor
    {
        private readonly Dictionary<Mark, IMarkProcessor> marks;

        public MarkdownProcessor(Dictionary<Mark, IMarkProcessor> marksInfo = null)
        {
            marks = new Dictionary<Mark, IMarkProcessor>
            {
                {new HeadMark(), new HeadProcessor()},
                {new StrongMark(), new StrongProcessor()},
                {new ItalicMark(), new ItalicProcessor()},
                {new LinkMark(), new LinkProcessor()}
            };
            if (marksInfo == null) 
                return;
            
            foreach (var (key, value) in marksInfo)
                marks.Add(key, value);
        }

        public List<TokenMd> FormatTokens(List<TokenMd> tokens)
        {
            return tokens.Select(FormatToken).ToList();
        }

        private TokenMd FormatToken(TokenMd token)
        {
            var resultToken = token;
            if (resultToken.InnerTokens != null && resultToken.InnerTokens.Count > 0)
                resultToken.InnerTokens = FormatTokens(resultToken.InnerTokens);
            
            if (resultToken.InnerTokens != null && resultToken.InnerTokens.Count > 0)
            {
                resultToken.FormattedText = JoinInnerTokensInToken(resultToken.InnerTokens);
                resultToken.InnerTokens = null;
            }
            else
                resultToken.FormattedText = resultToken.TokenWithoutMark;

            if (!(resultToken.Mark is EmptyMark))
                resultToken = marks[resultToken.Mark].FormatToken(resultToken);

            return resultToken;
        }

        private string JoinInnerTokensInToken(List<TokenMd> innerTokens)
        {
            var builder = new StringBuilder();

            foreach (var innerToken in innerTokens)
                builder.Append(
                    innerToken.InnerTokens != null && innerToken.InnerTokens.Count > 0
                    ? JoinInnerTokensInToken(innerToken.InnerTokens)
                    : innerToken.FormattedText);

            return builder.ToString();
        }
    }
}