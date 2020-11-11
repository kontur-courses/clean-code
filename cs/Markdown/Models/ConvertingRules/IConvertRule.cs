using Markdown.Models.Tags;

namespace Markdown.Models.ConvertingRules
{
    internal interface IConvertRule
    {
        Tag From { get; }
        Tag To { get; }
    }
}
