using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Md
    {
        private readonly TokenParser tokenParser;

        private readonly HtmlBuilder htmlBuilder;

        public Md(TokenParser tokenParser, HtmlBuilder htmlBuilder)
        {
            this.tokenParser = tokenParser;
            this.htmlBuilder = htmlBuilder;
        }

        public string Render(string text)
        {
            var tokens = tokenParser.Parse(text);
            return htmlBuilder.Build(tokens);
        }
    }
}
