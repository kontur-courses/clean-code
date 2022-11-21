using System.Collections.Generic;
using Markdown.Tags;

namespace Markdown
{
    public class Token
    {
        public string Name;
        public ITag Tag;
        public string Content = "";
        public List<Token> Children = new ();

        public Token(string name, ITag tag)
        {
            Name = name;
            Tag = tag;
        }
    }
}