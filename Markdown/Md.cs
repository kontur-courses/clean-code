using Markdown.MDParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Md
    {
        public static string Render(string text, IParser parser, IConverter converter)
        {
            var dom = parser.BuildDom(text);
            return converter.Convert(dom);
        }
    }
}
