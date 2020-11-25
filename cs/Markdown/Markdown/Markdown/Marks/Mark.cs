using System;
using System.Linq;

namespace Markdown
{
    public abstract class Mark
    {
        public string DefiningSymbol { get; protected set; }
        public string[] AllSymbols { get; protected set; }
        
        public (string startFormattedMark, string endFormattedMark) FormattedMarkSymbols { get; protected set; }
        
        protected bool Equals(Mark other)
        {
            return DefiningSymbol == other.DefiningSymbol;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Mark) obj);
        }

        public override int GetHashCode()
        {
            return (DefiningSymbol != null ? DefiningSymbol.GetHashCode() : 0);
        }

        public static bool operator ==(Mark left, Mark right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Mark left, Mark right)
        {
            return !Equals(left, right);
        }
    }
}