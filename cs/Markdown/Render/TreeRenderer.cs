using System.Text;
using Markdown.TreeBuilder.Nodes;

namespace Markdown.Render;

public class TreeRenderer : ITreeRenderer
{
	public string Render(Node tokens)
	{
		var sb = new StringBuilder();
		foreach (var token in tokens.Children)
			sb.Append(RenderToken(token));

		return sb.ToString();
	}

	private string? RenderToken(Node node)
	{
		return node switch
		{
			TextNode textNode => textNode.Value,
			_ => $"{node.OpenTag}{Render(node)}{node.CloseTag}"
		};
	}
}