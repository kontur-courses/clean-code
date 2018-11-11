using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Elements;
using Markdown.Parsers;

namespace Markdown
{
    public class Md
    {
        public string Render(string markdown)
        {
            ParsingResult result = EmphasisParser.ParseElement(markdown, 0, RootElementType.Create());
            return HtmlRender.RenderToHtml(result.Element);
        }
    }
}
