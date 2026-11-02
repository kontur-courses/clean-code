using Markdown.Parser;
using Markdown.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class Md
    {
        public static string Render(string text)
        {
            return HTMLRender.Render(
                    FileParser.SplitTextForParagraph(text)
                    .Select(paragraphText => ParagraphParser.CreateParagraph(paragraphText)));
        }
    }
}
