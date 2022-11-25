namespace Markdown.Tags;

public class MarkdownTag
{
	public MarkdownTag(string open, string? close)
	{
		Open = open;
		Close = close;
	}

	public MarkdownTag(string name) : this(name, name)
	{
	}

	public string Open { get; }

	public string? Close { get; }

	//public static MarkdownTag Italic => new("_");
	//public static MarkdownTag Bold => new("__");
	//public static MarkdownTag Header => new("# ", string.Empty);

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