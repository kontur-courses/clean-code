using System.Text;

namespace Markdown.TagHandlers
{
    public interface IHtmlTagCreator
    {
        Tag GetHtmlTag(StringBuilder markdownText, int openTagIndex);
    }
}