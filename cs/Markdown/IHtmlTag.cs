using System.Text;

namespace Markdown
{
    public interface IHtmlTagCreator
    {
        StringBuilder GetHtmlTag(string markdownText, int openTagIndex);
    }
}