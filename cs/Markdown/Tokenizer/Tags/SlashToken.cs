namespace Markdown.Tokenizer.Tags;

public class SlashToken : Token
{
	public override TokenType TokenType => TokenType.Slash;

	public SlashToken()
	{
		Value = "\\";
	}
}