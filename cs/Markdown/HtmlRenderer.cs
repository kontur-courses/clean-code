using System.Text;
using Markdown.Abstractions;
using Markdown.Extensions;
using Markdown.Primitives;

namespace Markdown;

public class HtmlRenderer : IRenderer
{
    public string Render(IEnumerable<TagNode> tagNodes)
    {
        if (tagNodes == null)
        {
            throw new ArgumentNullException(nameof(tagNodes));
        }
        
        return string.Join("", tagNodes.Select(ToHtml));
    }

    private string ToHtml(TagNode node)
    {
        if (node.Tag.Type == TagType.Text)
        {
            return node.Tag.Value;
        }
        var sb = new StringBuilder();
        var tag = Tag.GetName(node.Tag.Type);

        sb.Append(node.Tag.Type == TagType.Link
            ? $"<{tag} href=\"{node.Tag.Value}\">"
            : $"<{tag}>");

        if (node.Children.Length > 0)
        {
            sb.Append(string.Join("", node.Children.Select(ToHtml)));
        }

        sb.Append($"</{tag}>");

        return sb.ToString();
    }
}