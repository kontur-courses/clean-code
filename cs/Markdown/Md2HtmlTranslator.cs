using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md2HtmlTranslator
    {
        public string TranslateMdToHtml(string mdText, IEnumerable<Paragraph> paragraphs)
        {
            var htmlBuilder = new StringBuilder();
            foreach (var paragraph in paragraphs)
            {
                var lastPosition = 0;
                var tags = paragraph.ValidTokens.OrderBy(t => t.TokenPosition).ThenBy(t => t.TokenType.Template.Length);
                foreach (var htmlTag in tags)
                {
                    htmlBuilder.Append(paragraph.MdText.Substring(lastPosition, htmlTag.TokenPosition - lastPosition));
                    htmlBuilder.Append(WrapHtmlTagInBrackets(htmlTag));

                    var shift = htmlTag.TokenType.TokenLocationType == TokenLocationType.InlineToken ||
                                htmlTag.TokenType.TokenLocationType == TokenLocationType.StartingToken
                        ? htmlTag.TokenType.Template.Length
                        : 0;

                    lastPosition = htmlTag.TokenPosition + shift;
                }
            }

            return htmlBuilder.ToString();
        }
        private string WrapHtmlTagInBrackets(SingleToken htmlTag)
        {
            switch (htmlTag.LocationType)
            {
                case LocationType.Opening:
                case LocationType.Single:
                    return ($"<{htmlTag.TokenType.HtmlTag}>");
                case LocationType.Closing:
                    return ($"</{htmlTag.TokenType.HtmlTag}>");
                default:
                    throw new InvalidOperationException("Invalid token location type");
            }
        }
    }
}
