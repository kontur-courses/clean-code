namespace Markdown
{
    public class Token
    {
        public Token(string text, int position)
        {
            Text = text;
            Position = position;
        }

        public string Text { get; }
        public int Position { get; }
    }
}