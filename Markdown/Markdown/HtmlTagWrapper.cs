using System.Collections.Generic;
using System.Text;
using Markdown.Extensions;
using Markdown.Tag;

namespace Markdown
{
    public class HtmlTagWrapper
    {
        public readonly List<string> symbols;

        public HtmlTagWrapper(List<string> symbols)
        {
            this.symbols = symbols;
        }

        public string ConvertToHtml(List<ITag> pairedTags)
        {
            var htmlBuilder = new StringBuilder();

            foreach (var pairedTag in pairedTags)
                htmlBuilder.Append(Wrap(pairedTag));

            return htmlBuilder.ToString();
        }

        private string Wrap(ITag tag) => tag.Type == MdType.Text
            ? tag.Content.RemoveEscapedSymbols(symbols)
            : $"<{tag.Html}>{GetInnerFormattedText(tag)}</{tag.Html}>";

        private string GetInnerFormattedText(ITag tag)
        {
            var md = new Md(tag.AllowedInnerTypes);
            return md.Render(tag.Content);
        }
    }
}