using Markdown.Core.Tags;

namespace Markdown.Core.Rules
{
    interface IRule
    {
        ITag SourceTag { get; }
        ITag ResultTag { get; }
    }
}