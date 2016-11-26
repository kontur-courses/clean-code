namespace Markdown
{
    public class Attribute
    {
        public AttributeType Type { get; set; }
        public string Value { get; set; }

        public Attribute(string value, AttributeType type)
        {
            Value = value;
            Type = type;
        }
    }

    public enum AttributeType
    {
        Url,
        Style
    }
}
