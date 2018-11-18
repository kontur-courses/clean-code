using System.Collections.Generic;
using System.Text;
using Markdown.Tag;

namespace Markdown
{
    public class HtmlTagWrapper
    {
        public readonly Dictionary<string, ITag> dictionaryTags;

        public HtmlTagWrapper(Dictionary<string, ITag> dictionaryTags)
        {
            this.dictionaryTags = dictionaryTags;
        }
        public string ConvertToHtml(List<ITag> pairedTags)
        {
            var htmlBuilder = new StringBuilder();

            foreach (var pairedTag in pairedTags)
                htmlBuilder.Append(Wrap(pairedTag));

            return htmlBuilder.ToString();
        }

        private string Wrap(ITag tag) => tag.Type == MdType.Text
            ? tag.Content.RemoveEscapedSymbols(dictionaryTags)
            : $"<{tag.Html}>{GetInnerFormattedText(tag)}</{tag.Html}>";

        private string GetInnerFormattedText(ITag tag)
        {
            var md = new Md(dictionaryTags);
            return md.Render(tag);
        }
    }
}