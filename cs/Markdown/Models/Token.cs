using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Token
    {
        public TagInfo Tag { get; }
        public string Content { get; }

        public Token(TagInfo tag, string content)
        {
            Tag = tag;
            Content = content;
        }
    }
}
