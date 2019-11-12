using System.Collections.Generic;
using Markdown.Core.Tags;
using Markdown.Core.Tags.HtmlTags;
using Markdown.Core.Tags.MarkdownTags;

namespace Markdown.Core.Processors
{
    class SingleUnderscoreProcessor : BaseParser
    {
        public override IHtmlTag HtmlTag { get; }
        public override IMarkdownTag MarkdownTag { get; }

        public SingleUnderscoreProcessor()
        {
            HtmlTag = new Em();
            MarkdownTag = new SingleUnderscore();
        }

        protected override List<Token> FindMarkdownTags(string markdown)
        {
            return null;
        }
    }
}