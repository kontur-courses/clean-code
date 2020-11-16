using Markdown.Models.Tags;

namespace Markdown.Models.ConvertOptions.UnionRules
{
    internal interface IUnionRule
    {
        Tag Element { get; }
        Tag ToUnionWith { get; }
    }
}
