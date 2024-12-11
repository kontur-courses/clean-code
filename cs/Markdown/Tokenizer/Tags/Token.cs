namespace Markdown.Tokenizer.Tags;

public class Token
{
	public TagStatus TagStatus { get; set; }
	public virtual TokenType TokenType { get; }
	public string Value = string.Empty;
}