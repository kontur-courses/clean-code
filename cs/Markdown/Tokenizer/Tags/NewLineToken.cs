namespace Markdown.Tokenizer.Tags;

public class NewLineToken : Token
{
	public override TokenType TokenType => TokenType.NewLine;

	public NewLineToken()
	{
		Value = "\n";
	}
}