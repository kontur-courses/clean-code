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

        public List<Token> Children;

        public Token(TagInfo tag, List<Token> children)
        {
            Tag = tag;
            Children = children;
        }

        public Token(string content) 
        { 
            Content = content; 
        }
    }
}
