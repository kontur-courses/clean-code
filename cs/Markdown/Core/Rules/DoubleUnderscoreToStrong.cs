using Markdown.Core.Tags;
using Markdown.Core.Tags.HtmlTags;
using Markdown.Core.Tags.MarkdownTags;

namespace Markdown.Core.Rules
{
    internal class DoubleUnderscoreToStrong : IRule
    {
        public ITag SourceTag { get; }
        public ITag ResultTag { get; }

        public DoubleUnderscoreToStrong()
        {
            SourceTag = new DoubleUnderscore();
            ResultTag = new Strong();
        }
    }
}