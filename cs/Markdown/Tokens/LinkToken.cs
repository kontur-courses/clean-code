using System.Collections.Generic;
using System.Linq;

namespace Markdown.Tokens
{
    public class LinkToken : IToken
    {
        public string Link { get; }
        public List<IToken> Description { get; }

        public LinkToken(List<IToken> description, string link)
        {
            Description = description;
            Link = link;
        }

        public override int GetHashCode()
        {
            return Link.GetHashCode() ^ Description.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LinkToken objAsToken)) return false;
            if (!objAsToken.Link.Equals(this.Link)) return false;
            if (objAsToken.Description.Count != this.Description.Count) return false;
            return !Description.Where((t, i) => !t.Equals(objAsToken.Description[i])).Any();
        }
    }
}