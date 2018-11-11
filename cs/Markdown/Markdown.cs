using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Markdown
    {
        public string Render(string paragraph)
        {
            var markupsList = new List<Markup>();

            var markups = new MarkupFinder(markupsList).GetMarkupsWithPositions();
            return new Md2HtmlTranslator(markupsList).TranslateMdToHtml(paragraph, markups);
        }
    }
}
