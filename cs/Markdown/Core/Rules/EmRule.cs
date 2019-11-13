using Markdown.Core.Tags;
using Markdown.Core.Tags.HtmlTags;
using Markdown.Core.Tags.MarkdownTags;

namespace Markdown.Core.Rules
{
    class EmRule : IRule
    {
        public ITag sourceTag { get; }
        public ITag resultTag { get; }

        public EmRule()
        {
            sourceTag = new SingleUnderscore();
            resultTag = new Em();
        }
    }
}