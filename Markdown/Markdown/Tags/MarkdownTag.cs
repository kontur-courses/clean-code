using Markdown.Tokens;

namespace Markdown.Tags;

public class MarkdownTag
{
	public MarkdownTag(string open, string? close, TokenType type)
	{
		Open = open;
		Close = close;
		Type = type;
	}

	public MarkdownTag(string name, TokenType type) : this(name, name, type)
	{
	}

	public string Open { get; }

	public string? Close { get; }

	public TokenType Type { get; }

	public override bool Equals(object? obj)
	{
		if (obj is not MarkdownTag other) return false;
		return Open.Equals(other.Open) && Close.Equals(other.Close);
	}

	public override int GetHashCode()
	{
		return $"{Open}{Close}".GetHashCode();
	}
}