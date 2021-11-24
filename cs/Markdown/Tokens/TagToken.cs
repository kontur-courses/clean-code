using System.Linq;
using Markdown.Tags;

namespace Markdown.Tokens
{
    public class TagToken : Token
    {
        private readonly Tag _tag;
        protected override bool AllowInners => _tag.AllowNesting;

        public TagToken(string content, Tag tag) : base(content)
        {
            _tag = tag;
        }

        public override string Render()
        {
            if (_inners.Count == 0)
                return _tag.OpenHtmlTag + Content + _tag.CloseHtmlTag;
            return _tag.OpenHtmlTag
                + string.Join("", _inners.Select(inner => inner.Render()))
                + _tag.CloseHtmlTag;
        }
    }
}
