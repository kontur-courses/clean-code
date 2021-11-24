using System.Linq;
using Markdown.Tags;

namespace Markdown
{
    public class TagToken : Token
    {
        public readonly Tag Tag;
        public override bool AllowInners => Tag.AllowNesting;

        public TagToken(string content, Tag tag) : base(content)
        {
            Tag = tag;
        }

        public override string Render()
        {
            if (_inners.Count == 0)
                return Tag.OpenHtmlTag + Content + Tag.CloseHtmlTag;
            return Tag.OpenHtmlTag
                + string.Join("", _inners.Select(inner => inner.Render()))
                + Tag.CloseHtmlTag;
        }
    }
}
