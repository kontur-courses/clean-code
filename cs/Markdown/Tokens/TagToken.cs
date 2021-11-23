using System.Linq;
using System.Text;
using Markdown.Tags;

namespace Markdown
{
    public class TagToken : Token
    {
        public readonly Tag Tag;
        public override bool AllowInners => Tag.AllowNesting;

        public TagToken(int begin, int end, Tag tag) : base(begin, end)
        {
            Tag = tag;
        }

        public override string Render(string str)
        {
            var content = str.Substring(Begin, Length);
            if (_inners.Count > 0)
            {
                _inners = _inners.OrderBy(token => token.Begin).ToList();
                var renderedContent = new StringBuilder();

                var first = _inners.First() as TagToken;
                var last = _inners.Last() as TagToken;
                if (first.Begin != 0)
                    renderedContent.Append(content.Substring(0, first.Begin - first.Tag.OpenMdTag.Length));
                СompensateLosesParentParts();
                for (int i = 0; i < _inners.Count; i++)
                    renderedContent.Append(_inners[i].Render(content));

                if (last.End != content.Length)
                    renderedContent.Append(content.Substring(last.End + last.Tag.CloseMdTag.Length, content.Length - last.End - last.Tag.CloseMdTag.Length));

                return Tag.OpenHtmlTag + renderedContent + Tag.CloseHtmlTag;
            }
            else
                return Tag.OpenHtmlTag + content + Tag.CloseHtmlTag;
        }

        private void СompensateLosesParentParts()
        {
            var loses = _inners
                .Select(token => token as TagToken)
                .OrderBy(token => token.Begin)
                .Zip(_inners.OrderBy(token => token.Begin).Select(token => token as TagToken).Skip(1), (first, second) => (first, second))
                .Select(tuple => (End: tuple.first.End + 1, Begin: tuple.second.Begin - tuple.second.Tag.OpenMdTag.Length))
                .Where(tuple => tuple.End != tuple.Begin)
                .Select(tuple => new StringToken(tuple.End, tuple.Begin) as Token);

            _inners = _inners
                .Union(loses)
                .OrderBy(token => token.Begin)
                .ToList();
        }
    }
}
