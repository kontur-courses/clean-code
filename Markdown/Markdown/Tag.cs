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
        public List<TagValue> CantBeInside { get; }
        public Tag(TagValue value, string open, string close, List<TagValue> cantBeInside=null)
        {
            Value = value;
            Open = open;
            Close = close;
            CantBeInside = cantBeInside ?? new List<TagValue>();
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
                   Value == otherTag.Value && CantBeInside.Equals(otherTag.CantBeInside);
        }
    }
}
