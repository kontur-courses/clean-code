using System.Text;

namespace Markdown
{
    public interface IHtmlTagCreator
    {
        (StringBuilder, int) GetHtmlTag(string markdownText, int openTagIndex);
    }
}