namespace Markdown
{
    public class Token
    {
        public readonly Tag Tag;
        public readonly int Position;
        public readonly bool IsOpen;

        public Token(Tag tag, int position, bool isOpen = true)
        {
            Tag = tag;
            Position = position;
            IsOpen = isOpen;
        }
    }
}
