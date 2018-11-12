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
            markupsList.Add(new Markup("simpleUnderscore", "_", "em"));
            markupsList.Add(new Markup("doubleUnderscore", "__", "strong"));

            var markups = new MarkupFinder(markupsList).GetMarkupsWithPositions(paragraph);
            return new Md2HtmlTranslator(markupsList).TranslateMdToHtml(paragraph, markups);
        }
    }
}
