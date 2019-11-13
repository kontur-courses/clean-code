using Markdown.Core.Tags;
using Markdown.Core.Tags.HtmlTags;
using Markdown.Core.Tags.MarkdownTags;

namespace Markdown.Core.Rules
{
    class StrongRule : IRule
    {
        public ITag sourceTag { get; }
        public ITag resultTag { get; }

        public StrongRule()
        {
            sourceTag = new DoubleUnderscore();
            resultTag = new Strong();
        }
    }
}