using System.Collections.Generic;
using System.Linq;

namespace Markdown.Tokens
{
    public class PairedTagToken : IToken
    {
        public TokenType Type { get; } = TokenType.PairedTag;
        public string Name { get; }
        public List<IToken> Children { get; }

        public PairedTagToken(string name, List<IToken> children)
        {
            Name = name;
            Children = children;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PairedTagToken objAsToken)) return false;
            if (!(objAsToken.Type == this.Type && objAsToken.Name.Equals(this.Name))) return false;
            if (objAsToken.Children.Count != this.Children.Count) return false;
            return !Children.Where((t, i) => !t.Equals(objAsToken.Children[i])).Any();
        }
    }
}