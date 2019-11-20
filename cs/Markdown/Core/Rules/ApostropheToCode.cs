using Markdown.Core.Tags;
using Markdown.Core.Tags.HtmlTags;
using Markdown.Core.Tags.MarkdownTags;

namespace Markdown.Core.Rules
{
    class ApostropheToCode : IRule
    {
        public ITag SourceTag { get; }
        public ITag ResultTag { get; }

        public ApostropheToCode()
        {
            SourceTag = new Apostrophe();
            ResultTag = new Code();
        }
    }
}