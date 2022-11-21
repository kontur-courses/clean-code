namespace Markdown.Tags;

public class MarkdownTag
{
	public MarkdownTag(string withStart, string withEnd)
	{
		WithStart = withStart;
		WithEnd = withEnd;
	}

	public MarkdownTag(string name) : this(name, name)
	{
	}

	public string WithStart { get; }

	public string WithEnd { get; }

	//public static MarkdownTag Italic => new("_");
	//public static MarkdownTag Bold => new("__");
	//public static MarkdownTag Header => new("# ", string.Empty);

	public override bool Equals(object? obj)
	{
		if (obj is not MarkdownTag other) return false;
		return WithStart.Equals(other.WithStart) && WithEnd.Equals(other.WithEnd);
	}

	public override int GetHashCode()
	{
		return $"{WithStart}{WithEnd}".GetHashCode();
	}
}