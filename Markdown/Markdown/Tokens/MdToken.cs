namespace Markdown.Tokens;

public class MdToken : IToken
{
	public MdToken(string sourceText, int start, int end, TokenType type)
	{
		SourceText = sourceText;
		Start = start;
		End = end;
		Type = type;
	}

	public string SourceText { get; }
	public int Start { get; }
	public int End { get; }

	public string Value => Start == End ? string.Empty : SourceText.Substring(Start, End - Start + 1);

	public TokenType Type { get; }

	public IToken? nextToken { get; set; }

	public IToken? nestingTokens { get; set; }
}