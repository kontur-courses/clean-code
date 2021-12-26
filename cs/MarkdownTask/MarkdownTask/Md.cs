using System.Linq;
using MarkdownTask.Searchers;
using MarkdownTask.Tags;

namespace MarkdownTask
{
    public class Md
    {
        private readonly ITagSearcher[] searchers;
        private readonly TagsInspector tagsInspector;

        public Md(ITagSearcher[] searchers)
        {
            this.searchers = searchers;
            tagsInspector = new TagsInspector();
        }

        public string Render(string mdText)
        {
            var tags = searchers
                .SelectMany(searcher => searcher.SearchForTags(mdText))
                .OrderBy(tag => tag.StartsAt)
                .ToList();

            var inspectedTags = tagsInspector
                .ExcludeIntersection(TagType.Italic, TagType.Strong)
                .ExcludeContaining(TagType.Italic, TagType.Strong)
                .InspectTags(tags);

            var htmlText = new Converter().ConvertMdToHtml(mdText, inspectedTags);
            return htmlText;
        }
    }
}