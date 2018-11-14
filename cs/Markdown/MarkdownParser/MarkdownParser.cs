using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class MarkdownParser
    {
        private string markdown;
        public StringReader stringReader;

        public MarkdownParser(string markdown, List<ISpanElement> spanElements)
        {
            this.markdown = markdown;
            stringReader = new StringReader(markdown, spanElements);
        }

        public IEnumerable<HtmlTag> ParseMarkdownOnHtmlTags()
        {
            while (stringReader.CurrentPosition < markdown.Length)
                yield return ParseNextTag();
        }

        public HtmlTag ParseNextTag()
        {
            var tagContent = String.Empty;
            var startPosition = stringReader.CurrentPosition;
            var spanElement = stringReader.DetermineSpanElement();

            if (spanElement == null)
            {
                tagContent = stringReader.ReadUntilTag();
                return new HtmlTag(tagContent);
            }

            var elementInfo = stringReader.GetClosingElementInfo(spanElement);

            if (elementInfo != null)
                return stringReader.WrapTag(elementInfo, spanElement);
            

            stringReader.CurrentPosition = startPosition + 1;
            tagContent = stringReader.ReadUntilTag();

            return new HtmlTag(String.Concat(markdown.ElementAt(startPosition), tagContent));
        }
        

    }
}
