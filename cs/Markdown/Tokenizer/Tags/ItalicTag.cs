namespace Markdown.Tokenizer.Tags;

public class ItalicTag : Token
{
	public override TokenType TokenType => TokenType.Italic;

	public ItalicTag(TagStatus tagStatus)
	{
		Value = "_";
		TagStatus = tagStatus;
	}
}