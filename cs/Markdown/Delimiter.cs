namespace Markdown
{
    internal class Delimiter
    {
        public Delimiter(string value, int position)
        {
            Value = value;
            Position = position;
        }

        public bool IsClosing { get; set; }

        public bool IsOpening { get; set; }

        public Delimiter Partner { get; set; }

        public int Position { get; }

        public string Value { get; }

        public override bool Equals(object obj)
        {
            if (obj is Delimiter delimiter)
                return Position == delimiter.Position && Value.Equals(delimiter.Value);
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Position * 397) ^ Value.GetHashCode();
            }
        }
    }
}
