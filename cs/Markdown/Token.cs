namespace Markdown
{
    public class Token
    {
        private int Position { get; set; }
        private int Lenght { get; set; }
        private TagType Type { get; set; }

        public Token() {}
    }
}