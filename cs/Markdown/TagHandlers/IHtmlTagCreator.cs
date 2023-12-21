using System.Text;

namespace Markdown.TagHandlers
{
    public interface IHtmlTagCreator
    {
        bool IsTagSymbol(StringBuilder markdownText, int i);

        Tag FindTag(StringBuilder markdownText, int currentIndex, FindTagSettings settings,
            string? closingTagParent);
    }
}