using System.Collections.Generic;
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
            if (marksInfo != null)
                foreach (var markInfo in marksInfo)
                    marks.Add(markInfo.Key, markInfo.Value);
        }

        public List<TokenMd> FormatTokens(List<TokenMd> tokens)
        {
            var formattedTokens = new List<TokenMd>();

            for (var i = 0; i < tokens.Count; i++)
                formattedTokens.Add(FormatToken(tokens[i]));

            return formattedTokens;
        }

        private List<TokenMd> GetNewInnerTokens(List<TokenMd> innerTokens)
        {
            var resultInnerToken = innerTokens;
            for (var i = 0; i < innerTokens.Count; i++)
                resultInnerToken[i] = FormatToken(resultInnerToken[i]);

            return resultInnerToken;
        }

        private TokenMd FormatToken(TokenMd token)
        {
            var resultToken = token;
            if (resultToken.InnerTokens != null && resultToken.InnerTokens.Count > 0)
                resultToken.InnerTokens = GetNewInnerTokens(resultToken.InnerTokens);


            if (resultToken.InnerTokens != null && resultToken.InnerTokens.Count > 0)
            {
                resultToken.FormattedText = JoinInnerTokensInToken(resultToken.InnerTokens);
                resultToken.InnerTokens = null;
            }
            else
            {
                resultToken.FormattedText = resultToken.TokenWithoutMark;
            }

            if (resultToken.Mark != null)
                resultToken = marks[resultToken.Mark].FormatToken(resultToken);

            return resultToken;
        }

        private string JoinInnerTokensInToken(List<TokenMd> innerTokens)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < innerTokens.Count; i++)
                if (innerTokens[i].InnerTokens != null && innerTokens[i].InnerTokens.Count > 0)
                    builder.Append(JoinInnerTokensInToken(innerTokens[i].InnerTokens));
                else
                    builder.Append(innerTokens[i].FormattedText);

            return builder.ToString();
        }
    }
}