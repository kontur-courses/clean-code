using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Tag
    {
        public string Name { get; }
        public string Open { get; }
        public string Close { get; }
        public bool CanBeInside { get; }

        public Tag(string name, string open, string close, bool canBeInside=true)
        {
            Name = name;
            Open = open;
            Close = close;
            CanBeInside = canBeInside;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ (Open.GetHashCode() * Close.GetHashCode());
            
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var otherTag = (Tag)obj;
            return Open == otherTag.Open && Close == otherTag.Open &&
                   Name == otherTag.Name && CanBeInside == otherTag.CanBeInside;
        }
    }
}
