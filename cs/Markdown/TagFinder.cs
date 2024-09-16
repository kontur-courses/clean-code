using Markdown.TagHandlers;
using System.Text;

namespace Markdown
{
    public class TagFinder
    {
        public TagFinder(List<IHtmlTagCreator> listHandlers)
        {
            this.listHandlers = listHandlers;
        }

        private readonly List<IHtmlTagCreator> listHandlers;

        public Tag? FindTag(StringBuilder markdownText, int currentIndex, FindTagSettings settings, string? closingTagParent)
        {
            foreach (var handler in listHandlers)
                if (handler.IsTagSymbol(markdownText, currentIndex))
                    return handler.FindTag(markdownText, currentIndex, settings, closingTagParent);
            
            return null;
        }
    }
}