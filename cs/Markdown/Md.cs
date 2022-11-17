using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Renders;
using Markdown.Parsers;

namespace Markdown
{
    public class Md
    {
        private readonly IRender render;
        public Md(IRender render)
        {
            this.render = render;
        }

        public string Render(string markdownText)
        {
            if (string.IsNullOrEmpty(markdownText))
                return markdownText;

            var markdownParser = new MarkdownParser(markdownText);
            var markdownTextTags = markdownParser.GetTags();
            var htmlTextTags = markdownTextTags.ConvertAll(tag=>tag.ToHtml());
            return render.Render(htmlTextTags);
        }
    }
}