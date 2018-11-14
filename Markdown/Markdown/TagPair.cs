using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TagPair
    {
        public string InitialOpen { get; }
        public string InitialClose { get; }
        public string FinalOpen { get; }
        public string FinalClose { get; }

        public TagPair(string initialStart, string initialEnd, string finalOpen, string finalClose)
        {
            InitialOpen = initialStart;
            InitialClose = initialEnd;
            FinalOpen = finalOpen;
            FinalClose = finalClose;
        }

        public override int GetHashCode()
        {
            return (InitialOpen.GetHashCode() ^ InitialClose.GetHashCode()) *
                   (FinalOpen.GetHashCode() ^ FinalClose.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var otherTag = (TagPair) obj;
            return InitialOpen == otherTag.InitialOpen && InitialClose == otherTag.InitialClose &&
                   FinalOpen == otherTag.FinalOpen && FinalClose == otherTag.FinalClose;
        }

    }
}
