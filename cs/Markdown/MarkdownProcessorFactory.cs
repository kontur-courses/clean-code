using System.Collections.Generic;
using Markdown.Html;

namespace Markdown
{
    public class MarkdownProcessorFactory
    {
        private static readonly IReadOnlyDictionary<string, PairedHtmlTag> SeparatorHtmlTags =
            new Dictionary<string, PairedHtmlTag>
            {
                {"_", PairedHtmlTag.Italic},
                {"__", PairedHtmlTag.Bold}
            };

        public static MarkdownProcessor Create()
        {
            return Create(new HtmlConverter(SeparatorHtmlTags));
        }

        public static MarkdownProcessor Create(IHtmlConverter htmlConverter)
        {
            return new MarkdownProcessor(htmlConverter);
        }
    }
}