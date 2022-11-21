using Markdown.Tokens;

namespace Markdown.TokenParsers;

public interface ITokenParser
{
	public IToken Parse(string text);
}