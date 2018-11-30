using System.Text;


namespace Markdown
{
    class Token
    {
        public int Start { get; }
        public Tag  Tag { get; }
        public StringBuilder Content { get; }

        public Token(int start, Tag tag)
        {
            Start = start;
            Tag = tag;
            Content = new StringBuilder();
        }
    }
}
