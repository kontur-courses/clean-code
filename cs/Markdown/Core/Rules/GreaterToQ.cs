using Markdown.Core.Tags;
using Markdown.Core.Tags.HtmlTags;
using Markdown.Core.Tags.MarkdownTags;

namespace Markdown.Core.Rules
{
    class GreaterToQ : IRule
    {
        public ITag SourceTag { get; }
        public ITag ResultTag { get; }

        public GreaterToQ()
        {
            SourceTag = new Greater();
            ResultTag = new Q();
        }
    }
}