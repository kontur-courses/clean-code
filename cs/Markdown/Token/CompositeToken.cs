using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class CompositeToken : Token
    {
        private readonly List<Token> innerTokens = new List<Token>();

        public CompositeToken(string value, int position, MdWrapSetting wrapSetting, IEnumerable<Token> tokens)
            : base(value, position, wrapSetting)
        {
            innerTokens.AddRange(tokens);
        }

        public override string Render()
        {
            var sb = new StringBuilder(Value);
            foreach (var token in innerTokens.OrderByDescending(t => t.Position))
            {
                sb.Remove(token.Position, token.Value.Length);
                sb.Insert(token.Position, token.Render());
            }
            return Wrap(sb.ToString());
        }
    }
}