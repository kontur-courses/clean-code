using System.Collections.Generic;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string paragraph)
        {
            var markups = new MarkupFinder().GetMarkupsWithPositions(paragraph);
            return new Md2HtmlTranslator().TranslateMdToHtml(paragraph, markups);
        }
    }
}
