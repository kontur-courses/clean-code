using Markdown.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class HtmlConvector : IMarkupConverter
    {
        public string Convert(IEnumerable<IToken> inputTagTokens)
        {
            var stringBuilder = new StringBuilder();

            foreach (var tag in inputTagTokens)
            {
                stringBuilder.Append(tag.ConvertToHtml());
            }

            return stringBuilder.ToString();
        }
    }
}