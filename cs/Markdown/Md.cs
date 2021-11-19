using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Md
    {
        public string Render(string rawInput)
        {
            var parsedInput = MarkdownParser.Parse(rawInput);
            return "";
        }
    }
}
