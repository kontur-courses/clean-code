using System.Linq;

namespace Markdown.Tokens
{
    public class StringToken : Token
    {
        protected override bool AllowInners => false;

        public StringToken(string content) : base(content)
        {
        }

        public override string Render()
        {
            return _inners.Count == 0 ? Content : string.Join("", _inners.Select(inner => inner.Render()));
        }
    }
}
