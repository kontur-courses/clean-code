namespace Markdown.Tokens;

public interface IToken
{
	public string Value { get; }

	public TokenType Type { get; }

	public IToken? nextToken { get; set; }

	public IToken? nestingTokens { get; set; }
}