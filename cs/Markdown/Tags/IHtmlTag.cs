using System.Text;

namespace Markdown.Tags
{
    public interface IHtmlTagCreator
    {
        (StringBuilder, int) GetHtmlTag(string markdownText, int openTagIndex);
    }
}