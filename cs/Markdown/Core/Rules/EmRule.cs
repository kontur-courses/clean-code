using Markdown.Core.Tags;
using Markdown.Core.Tags.HtmlTags;
using Markdown.Core.Tags.MarkdownTags;

namespace Markdown.Core.Rules
{
    class EmRule : IRule
    {
        public ITag SourceTag { get; }
        public ITag ResultTag { get; }

        public EmRule()
        {
            SourceTag = new SingleUnderscore();
            ResultTag = new Em();
        }
    }
}