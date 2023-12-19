using System.Text;

namespace Markdown.Tags
{
    public interface IHtmlTagCreator
    {
        Tag GetHtmlTag(string markdownText, int openTagIndex);
    }
}