using System.Linq;
using MarkdownTask.Searchers;

namespace MarkdownTask
{
    public class Md
    {
        private readonly ITagSearcher[] searchers;

        public Md(ITagSearcher[] searchers)
        {
            this.searchers = searchers;
        }

        public string Render(string mdText)
        {
            var tags = searchers
                .SelectMany(searcher => searcher.SearchForTags(mdText))
                .OrderBy(tag => tag.StartsAt)
                .ToList();
            var htmlText = new Converter().ConvertMdToHtml(mdText, tags);
            return htmlText;
        }
    }
}