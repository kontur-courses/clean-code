using Markdown.Core.Tags;
using Markdown.Core.Tags.HtmlTags;
using Markdown.Core.Tags.MarkdownTags;

namespace Markdown.Core.Rules
{
    internal class SharpToH1 : IRule
    {
        public ITag SourceTag { get; }
        public ITag ResultTag { get; }

        public SharpToH1()
        {
            SourceTag = new Sharpe();
            ResultTag = new H1();
        }
    }
}