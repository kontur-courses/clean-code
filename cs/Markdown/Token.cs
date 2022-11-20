using System.Collections.Generic;
using Markdown.Enums;
using Markdown.Tags;

namespace Markdown
{
    public class Token
    {
        public string Name;
        public ITag Tag;
        public TagState State;
        public string Content = "";
        public List<Token> Children = new ();

        public Token(string name, ITag tag, TagState state)
        {
            Name = name;
            Tag = tag;
            State = state;
        }
    }
}