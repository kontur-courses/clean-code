namespace Markdown.Tokenizer.Tags;

public class HeaderTag : Token
{
	public override TokenType TokenType => TokenType.Header;

	public HeaderTag()
	{
		TagStatus = TagStatus.Single;
		Value = "# ";
	}
}