namespace Markdown.Tokenizer.Tags;

public class TextToken : Token
{
	public override TokenType TokenType => TokenType.String;

	public TextToken(string value)
	{
		Value = value;
	}
}