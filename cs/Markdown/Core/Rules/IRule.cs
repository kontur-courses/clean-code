using Markdown.Core.Tags;

namespace Markdown.Core.Rules
{
    public interface IRule
    {
        ITag SourceTag { get; }
        ITag ResultTag { get; }
    }
}