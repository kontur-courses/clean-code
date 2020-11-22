namespace Markdown.Core
{
    public class Part
    {
        public string Value { get; set; }
        public bool Escaped { get; set; }

        public Part(string value, bool escaped)
        {
            Value = value;
            Escaped = escaped;
        }

        public Part(string value) : this(value, false) { }
        public Part(bool escaped) : this(null, escaped) { }
    }
}
