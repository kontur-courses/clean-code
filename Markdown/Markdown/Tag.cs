using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Tag
    {
        private List<TagType> canBeInside;
        public static Tag Empty => new Tag(TagType.None, "", "");
        public TagType Type { get; }
        public string Open { get; }
        public string Close { get; }
        public IReadOnlyList<TagType> CanBeInside => canBeInside;
        public Tag(TagType type, string open, string close, IEnumerable<TagType> canBeInside = null)
        {
            Type = type;
            Open = open;
            Close = close;
            this.canBeInside = new List<TagType>();
            if (canBeInside != null)
                this.canBeInside.AddRange(canBeInside);
            this.canBeInside.Add(TagType.None);
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ (Open.GetHashCode() * Close.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var otherTag = (Tag)obj;
            return Open == otherTag.Open && Close == otherTag.Open &&
                   Type == otherTag.Type && CanBeInside.Equals(otherTag.CanBeInside);
        }
    }
}
