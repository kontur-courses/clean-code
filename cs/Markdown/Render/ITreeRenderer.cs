using Markdown.TreeBuilder.Nodes;

namespace Markdown.Render;

public interface ITreeRenderer
{
	string Render(Node tokens);
}