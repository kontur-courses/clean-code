using System.Text;


namespace MarkdownNew
{
    class Token
    {
        public int Start { get; private set; }
        public Tag  Tag { get; private set; }
        public StringBuilder Content { get; private set; }
        public int Lenght { get; private set; }
        public int End => Start + Lenght - 1;
        public Token(int start, Tag tag)
        {
            this.Start = start;
            this.Tag = tag;
            Content = new StringBuilder();
        }

        public void SetEndOfToken(int position)
        {
            Lenght = position - Start + 1;
        }
    }
}
