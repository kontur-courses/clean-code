using System.Collections.Immutable;

namespace Markdown
{
    public class Token
    {
        public Token(string[] text, int position)
        {
            Words = text.ToImmutableList();
            Text = string.Join("", text);
            Position = position;
        }

        public string Text { get; }
        public int Position { get; }
        public ImmutableList<string> Words { get; }
    }
}