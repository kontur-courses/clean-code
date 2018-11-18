using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Tag
    {
        public TagType Type { get; }
        public string Open { get; }
        public string Close { get; }
        public List<TagType> CanBeInside { get; }

        public Tag(TagType type, string open, string close, List<TagType> canBeInside = null)
        {
            Type = type;
            Open = open;
            Close = close;
            CanBeInside = canBeInside ?? new List<TagType>();
            CanBeInside.Add(TagType.None);
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
