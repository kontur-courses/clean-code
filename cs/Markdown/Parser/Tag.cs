using System.Collections.Generic;
using Markdown.Md;

namespace Markdown
{
    public class Tag
    {
        public string Type;
        public string Value;
        public ICollection<Tag> Tags;

        public Tag()
        {
        }

        public Tag(string type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}