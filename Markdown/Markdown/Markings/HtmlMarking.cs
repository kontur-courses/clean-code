using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Markings
{
    public class HtmlMarking : IMarking<HtmlToken>
    {
        public IEnumerable<IEnumerable<HtmlToken>> TokensLines { get; }

        public HtmlMarking(IEnumerable<IEnumerable<HtmlToken>> tokensLines)
        {
            TokensLines = tokensLines;
        }

        public new string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach (var line in TokensLines)
            {
                foreach (var token in line)
                {
                    stringBuilder.Append(token.Value).Append(' ');
                }

                stringBuilder.Append('\n');
            }

            return stringBuilder.ToString();
        }
    }
}