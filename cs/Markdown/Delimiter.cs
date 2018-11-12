namespace Markdown
{
    public class Delimiter
    {
        public Delimiter(bool isPaired, string value, int position)
        {
            IsPaired = isPaired;
            Value = value;
            Position = position;
        }

        public int Position { get; }
        public string Value { get; }
        public bool IsPaired { get; }

    }
}
