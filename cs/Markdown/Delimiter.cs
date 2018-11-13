namespace Markdown
{
    public class Delimiter
    {
        public Delimiter(string value, int position)
        {
            Value = value;
            Position = position;
        }

        public int Position { get; }
        public string Value { get; }
        public bool IsFirst { get; set; }
        public bool IsLast { get; set; }

        public Delimiter Partner { get; set; }
    }
}
