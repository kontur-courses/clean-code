using System.Collections.Generic;

namespace Markdown
{
    class Md2HtmlTranslator
    {
        public Md2HtmlTranslator(List<Markup> markups)
        {
        }
        
        public string TranslateMdToHtml(string mdInput, Dictionary<Markup, List<MarkupPosition>> markups)
        {
            var htmlText = mdInput;
            foreach (var markupWithPosition in markups)
            {
                var markupTemplate = markupWithPosition.Key;
                foreach (var position in markupWithPosition.Value)
                {
                    htmlText = htmlText.Remove(position.Start, markupTemplate.Template.Length);
                    htmlText = htmlText.Insert(position.Start, $"<{markupTemplate.HTMLTag}>");
                    htmlText = htmlText.Remove(position.End, markupTemplate.Template.Length);
                    htmlText = htmlText.Insert(position.End, $"</{markupTemplate.HTMLTag}>");
                }
            }

            return htmlText;
        }
    }
}
