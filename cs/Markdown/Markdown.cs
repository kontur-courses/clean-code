using System.Collections.Generic;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string paragraph)
        {
            var markupsList = new List<Markup>();
            markupsList.Add(new Markup("doubleUnderscore", "__", "strong"));
            markupsList.Add(new Markup("simpleUnderscore", "_", "em"));

            var markups = new MarkupFinder(markupsList).GetMarkupsWithPositions(paragraph);
            return new Md2HtmlTranslator().TranslateMdToHtml(paragraph, markups);
        }
    }
}
