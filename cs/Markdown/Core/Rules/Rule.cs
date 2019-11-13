using Markdown.Core.Tags;

namespace Markdown.Core.Rules
{
    interface IRule
    {
        ITag sourceTag { get; }
        ITag resultTag { get; }
    }
}