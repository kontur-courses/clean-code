using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Md
    {
        private readonly IParser parser;
        private readonly IConverter converter;

        public Md()
        {
            parser = new MarkdownParser();
            converter = new HtmlConverter();
        }

        public string Render(string rowString)
        {
            return converter.Convert(rowString, parser.Parse(rowString));
        }
    }
}
