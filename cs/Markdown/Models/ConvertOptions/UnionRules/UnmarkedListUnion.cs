using Markdown.Models.Tags;
using Markdown.Models.Tags.HtmlTags;

namespace Markdown.Models.ConvertOptions.UnionRules
{
    internal class UnmarkedListUnion : IUnionRule
    {
        public Tag Element { get; }
        public Tag ToUnionWith { get; }

        public UnmarkedListUnion()
        {
            Element = new ListElement();
            ToUnionWith = new UnmarkedList();
        }
    }
}
