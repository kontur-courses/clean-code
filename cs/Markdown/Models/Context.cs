namespace Markdown.Models
{
    public class Context
    {
        public string Text { get; }
        public int Index { get; set; }

        public Context(string text, int index = 0)
        {
            Text = text;
            Index = index;
        }

        public bool HasNextSymbol() => Index + 1 < Text.Length;

        public char GetNextSymbol() => Text[Index + 1];

        public bool HasPreviousSymbol() => Index - 1 >= 0;

        public char GetPreviousSymbol() => Text[Index - 1];
    }
}