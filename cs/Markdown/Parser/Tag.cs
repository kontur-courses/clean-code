using System.Collections.Generic;

namespace Markdown
{
    public class Tag
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public bool IsRoot { get; set; }

        public ICollection<Tag> Tags { get; set; }
    }
}