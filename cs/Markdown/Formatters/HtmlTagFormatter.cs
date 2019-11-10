using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class HtmlTagFormatter : ITagFormatter
    {
        public string OnClose(string tag)
        {
            return $"</{tag}>";
        }

        public string OnOpen(string tag)
        {
            return $"<{tag}>";
        }
    }
}
