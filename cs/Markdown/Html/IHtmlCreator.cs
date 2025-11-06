using Markdown.Node;

namespace Markdown.Html;

public interface IHtmlCreator
{
    public string CreateHtml(INode node);
}