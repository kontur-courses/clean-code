using Markdown.Models.Tags;

namespace Markdown.Models.Rules
{
    internal interface IRule
    {
        Tag From { get; }
        Tag To { get; }
    }
}
