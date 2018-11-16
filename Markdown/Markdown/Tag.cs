using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Tag
    {
        public TagValue Value { get; }
        public string Open { get; }
        public string Close { get; }
        public bool CanBeInside { get; }

        public Tag(TagValue value, string open, string close, bool canBeInside=true)
        {
            Value = value;
            Open = open;
            Close = close;
            CanBeInside = canBeInside;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ (Open.GetHashCode() * Close.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var otherTag = (Tag)obj;
            return Open == otherTag.Open && Close == otherTag.Open &&
                   Value == otherTag.Value && CanBeInside == otherTag.CanBeInside;
        }
    }
}
