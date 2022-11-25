using Markdown.Tokens;

namespace Markdown.TokenRenderers;

public interface ITokenRenderer
{
	public string Render(IToken token);
}