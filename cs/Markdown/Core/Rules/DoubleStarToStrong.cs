using Markdown.Core.Tags;
using Markdown.Core.Tags.HtmlTags;
using Markdown.Core.Tags.MarkdownTags;

namespace Markdown.Core.Rules
{
    class DoubleStarToStrong : IRule
    {
        public ITag SourceTag { get; }
        public ITag ResultTag { get; }

        public DoubleStarToStrong()
        {
            SourceTag = new DoubleStar();
            ResultTag = new Strong();
        }
    }
}