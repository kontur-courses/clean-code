using Markdown.Core.Tags;
using Markdown.Core.Tags.HtmlTags;
using Markdown.Core.Tags.MarkdownTags;

namespace Markdown.Core.Rules
{
    class StrongRule : IRule
    {
        public ITag SourceTag { get; }
        public ITag ResultTag { get; }

        public StrongRule()
        {
            SourceTag = new DoubleUnderscore();
            ResultTag = new Strong();
        }
    }
}