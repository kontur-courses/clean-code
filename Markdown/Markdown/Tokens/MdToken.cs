namespace Markdown.Tokens;

public class MdToken : IToken
{
	public MdToken(string sourceText, int start, int end, TokenType type)
	{
		SourceText = sourceText;
		Start = Math.Max(0, start);
		End = end;
		Type = type;
	}

	public string SourceText { get; }
	public int Start { get; }
	public int End { get; }

	public string Value
	{
		get
		{
			if (Start == End) return String.Empty;
			if (Start == 0) return SourceText.Substring(Start, End);
			if (Type == TokenType.Escape) return SourceText.Substring(Start, End - Start + 1);
			return SourceText.Substring(Start, End - Start);
		}
	}

	public TokenType Type { get; }

	public IToken? nextToken { get; set; }

	public IToken? nestingTokens { get; set; }
}