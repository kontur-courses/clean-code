namespace Markdown.Attribute
{
    public class LinkAttribute : IAttribute
    {
        public LinkAttribute(string value, string name)
        {
            Value = value;
            Name = name;
        }

        public string Value { get; set; }
        public string Name { get; set; }
    }
}