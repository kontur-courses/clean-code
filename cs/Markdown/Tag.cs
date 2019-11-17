using System.Collections.Generic;

namespace Markdown
{
    public class Tag
    {
        public string Start { get; }
        public string End { get; }
        public IEnumerable<TagType> Children { get; }

        public Tag(string start, string end, IEnumerable<TagType> children)
        {
            Start = start;
            End = end;
            Children = children;
        }
    }
}