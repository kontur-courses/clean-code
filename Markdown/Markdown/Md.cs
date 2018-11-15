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

        public Md(IParser parser)
        {
            this.parser = parser;
        }

        public string Render(string rowString, Markup markup)
        {
            return parser.ParseTo(rowString, markup);
        }
    }
}
