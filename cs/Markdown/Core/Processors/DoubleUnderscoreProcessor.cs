using System;
using System.Collections.Generic;
using Markdown.Core.Tags;
using Markdown.Core.Tags.HtmlTags;
using Markdown.Core.Tags.MarkdownTags;

namespace Markdown.Core.Processors
{
    class DoubleUnderscoreProcessor : BaseParser
    {
        public override IHtmlTag HtmlTag { get; }
        public override IMarkdownTag MarkdownTag { get; }

        public DoubleUnderscoreProcessor()
        {
            HtmlTag = new Strong();
            MarkdownTag = new DoubleUnderscore();
        }

        protected override List<Token> FindMarkdownTags(string markdown)
        {
            throw new NotImplementedException();
        }
    }
}