using Markdown.Core.Tags;
using Markdown.Core.Tags.HtmlTags;
using Markdown.Core.Tags.MarkdownTags;

namespace Markdown.Core.Rules
{
    internal class StarToEm : IRule
    {
        public ITag SourceTag { get; }
        public ITag ResultTag { get; }

        public StarToEm()
        {
            SourceTag = new Star();
            ResultTag = new Em();
        }
    }
}