using Markdown.Models.Tags;

namespace Markdown.Models.ConvertOptions.ConvertRules
{
    internal interface IConvertRule
    {
        Tag From { get; }
        Tag To { get; }
    }
}
