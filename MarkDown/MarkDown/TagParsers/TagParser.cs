using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkDown.TagParsers
{
    public abstract class TagParser
    {
        public abstract string OpeningHtmlTag { get; }
        public abstract string ClosingHtmlTag { get; }
        public abstract string MdTag { get; }

        protected abstract List<MDToken> ParseLineOnMDTokens(string line);

        public string GetParsedLineFrom(string line)
        {
            var tokensToReplace = ParseLineOnMDTokens(line);
            var parsedLine = new StringBuilder();
            var indexNextToLastToken = 0;
            foreach (var token in tokensToReplace)
            {
                parsedLine.Append(line.Substring(indexNextToLastToken, token.StartIndex - indexNextToLastToken))
                    .Append(OpeningHtmlTag)
                    .Append(token.Value)
                    .Append(ClosingHtmlTag);
                indexNextToLastToken = token.GetIndexNextToToken();
            }
            parsedLine.Append(line.Substring(indexNextToLastToken, line.Length - indexNextToLastToken));
            return parsedLine.ToString();
        }
    }
}