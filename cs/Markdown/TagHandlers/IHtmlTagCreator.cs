using System.Text;

namespace Markdown.TagHandlers
{
    public interface IHtmlTagCreator
    {
        Tag FindTag(StringBuilder markdownText, int currentIndex, FindTagSettings settings,
            string? closingTagParent);

        bool IsTagSymbol(StringBuilder markdownText, int i);
    }
}