using Markdown.TokenParsers;
using Markdown.TokenRenderers;

namespace Markdown;

public class Md
{
	private readonly ITokenParser tokenParser = new MarkdownTokenParser();
	private readonly ITokenRenderer tokenRenderer = new HtmlTokenRenderer();

	public string Render(string markdownText)
	{
		var tokens = tokenParser.Parse(markdownText);

		return tokenRenderer.Render(tokens);
	}
}