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
                var tokens = paragraph.ValidTokens.OrderBy(t => t.TokenPosition).ThenBy(t => t.TokenType.TokenLocationType);

                SingleToken closingBoxToken = null;

                foreach (var token in tokens)
                {
                    if (token.TokenType.TokenLocationType == TokenLocationType.BoxesTokens && token.TokenPosition != 0)
                    {
                        closingBoxToken = token;
                        continue;
                    }

                    htmlBuilder.Append(paragraph.MdText.Substring(lastPosition, token.TokenPosition - lastPosition));
                    htmlBuilder.Append(WrapHtmlTagInBrackets(token));

                    var shift = token.TokenType.TokenLocationType == TokenLocationType.InlineToken
                        ? token.TokenType.Template.Length
                        : token.TokenType.TokenLocationType == TokenLocationType.StartingToken && token.TokenType.Template.Length != 0
                            ? token.TokenType.Template.Length + 1
                            : 0;

                    lastPosition = token.TokenPosition + shift;
                }

                if (closingBoxToken != null)
                {
                    htmlBuilder.Append(WrapHtmlTagInBrackets(closingBoxToken));
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
