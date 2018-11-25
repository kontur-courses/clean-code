using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md2HtmlTranslator
    {
        public string TranslateMdToHtml(string mdText, IEnumerable<HtmlTag> tokens)
        {
            var htmlText = GetHtmlText(mdText, tokens.OrderBy(token => token.Position));

            return htmlText;
        }

        private string GetHtmlText(string mdText, IEnumerable<HtmlTag> htmlTags)
        {
            var htmlBuilder = new StringBuilder();

            foreach (var htmlTag in htmlTags)
                htmlBuilder.Append(WrapHtmlTagInBrackets(htmlTag));

            return htmlBuilder.ToString();
        }

        private string WrapHtmlTagInBrackets(HtmlTag htmlTag)
        {
            if (htmlTag.Type == LocationType.Opening || htmlTag.Type == LocationType.Single)
                return ($"<{htmlTag.HtmlTemplate}>");
            if (htmlTag.Type == LocationType.Closing)
                return ($"</{htmlTag.HtmlTemplate}>");

            throw new InvalidOperationException("Invalid token location type");
        }
    }
}
