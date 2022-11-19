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
        private readonly IConverter converter;

        public Md(IConverter converter)
        {
            this.converter = converter;

        }
        public string Render(string markdownText)
        {
            return converter.Convert(markdownText);
        }
    }
}
    