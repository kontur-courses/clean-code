using System.Collections.Generic;
using System.Text;
using Markdown.Tag;

namespace Markdown
{
    public class HtmlTagWrapper
    {
        public readonly List<MdType> types;

        public HtmlTagWrapper(List<MdType> types)
        {
            this.types = types;
        }

        public string ConvertToHtml(List<ITag> pairedTags)
        {
            var htmlBuilder = new StringBuilder();

            foreach (var pairedTag in pairedTags)
                htmlBuilder.Append(Wrap(pairedTag));

            return htmlBuilder.ToString();
        }

        private string Wrap(ITag tag) => tag.Type == MdType.Text
            ? tag.Content.RemoveEscapedSymbols(types)
            : $"<{tag.Html}>{GetInnerFormattedText(tag)}</{tag.Html}>";

        private string GetInnerFormattedText(ITag tag)
        {
            var md = new Md(tag.AllowedInnerTypes);
            return md.Render(tag.Content);
        }
    }
}