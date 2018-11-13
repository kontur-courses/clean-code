using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.TextProcessing;

namespace Markdown
{
    public class Md
    {
        public string Render(string content)
        {
            var splitter = new TextSplitter(content);
            var tokens = splitter.SplitToTokens();
            var builder = new TextBuilder(tokens);
            var htmlCode = builder.BuildText();
            return htmlCode;
        }
    }
}
