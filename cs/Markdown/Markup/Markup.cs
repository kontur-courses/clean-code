using Markdown.Tags;

namespace Markdown.Markup
{
    public class Markup
    {
        public string Value { get; }
        public Tag Tag { get; }

        public Markup(string value, Tag tag)
        {
            Value = value;
            Tag = tag;
        }
    }
}