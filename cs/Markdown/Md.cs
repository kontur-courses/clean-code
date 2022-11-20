using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Converter;

namespace Markdown
{
    public class Md
    {
        private readonly IConverter converter = new MarkdownToHtmlConverter();

        public Md()
        {
        }

        public string Render(string markdownText)
        {
            return converter.Convert(markdownText);
        }
    }
}
    