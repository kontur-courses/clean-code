using System.Linq;

namespace Markdown
{
    public class StringToken : Token
    {
        public override bool AllowInners => false;

        public StringToken(string content) : base(content)
        {
        }

        public override string Render()
        {
            if (_inners.Count == 0)
                return Content;
            return string.Join("", _inners.Select(inner => inner.Render()));
        }
    }
}
