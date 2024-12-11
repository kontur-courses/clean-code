namespace Markdown.Tokenizer.Tags;

public enum TagStatus
{
	Open,
	Closed,
	Broken,
	Escaped,
	InWord,
	Single
}