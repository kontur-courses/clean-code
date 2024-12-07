using Markdown.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Md
    {
        private readonly IMarkdownParser _parser;
        private readonly IHtmlBuilder _converter;
        public Md(IMarkdownParser parser, IHtmlBuilder htmlBuilder)
        {
            _parser = parser;
            _converter = htmlBuilder;
        }

        public string Render(string markdownText)
        {
            if (string.IsNullOrEmpty(markdownText))
            {
                throw new ArgumentNullException("Provided string was empty");
            }

            var tokens = _parser.ParseMarkdown(markdownText);
            var htmlText = _converter.BuildHtmlFromMarkdown(markdownText, tokens);

            return htmlText;
        }
    }
}
