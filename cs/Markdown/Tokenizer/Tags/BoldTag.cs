namespace Markdown.Tokenizer.Tags;

public class BoldTag : Token
{
	public override TokenType TokenType => TokenType.Bold;

	public BoldTag(TagStatus tagStatus)
	{
		Value = "__";
		TagStatus = tagStatus;
	}
}