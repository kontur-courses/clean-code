using Markdown.Render;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Handlers;
using Markdown.TreeBuilder;

namespace Markdown;

public class MarkdownRenderer : IMarkdownRenderer
{
	private readonly ITreeRenderer treeRenderer = new TreeRenderer();
	private readonly ITokenizer tokenizer;

	private readonly List<IHandler> handlers = new()
	{
		new HeaderHandler(),
		new ItalicHandler(),
		new BoldHandler(),
	};

	public MarkdownRenderer()
	{
		tokenizer = new MarkdownTokenizer(new HandlerManager(handlers), new TagProcessor());
	}

	public string Render(string markdown)
	{
		var tokens = tokenizer.Tokenize(markdown);
		var tree = new TreeBuilder.TreeBuilder(new NodeFactory()).Build(tokens);

		return treeRenderer.Render(tree);
	}
}