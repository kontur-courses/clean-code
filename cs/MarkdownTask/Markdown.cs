using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownTask
{
    internal class Markdown
    {
        public readonly List<Token> tokens;

        ITagParser[] parsers =
        {
            new HeadingTagParser(),
            new ItalicTagParser(),
            new StrongTagParser()
        };
        public Markdown()
        {

        }

        public string Render(string markdownText)
        {
            throw new NotImplementedException();
        }
            
    }
}
