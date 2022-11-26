using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenParsers.MarkdownParsers;

public class DoubleTagParser : IMarkdownTagParser
{
	private readonly string? competingTagPattern;
	private readonly MarkdownTag tag;
	private List<MdToken> escapeTokens;
	private int paragraphEnd;
	private int paragraphStart;
	private string text;

	public DoubleTagParser(MarkdownTag tag, string? competingTagPattern = null)
	{
		if (tag.Close is null) throw new ArgumentException();
		this.tag = tag;
		this.competingTagPattern = competingTagPattern;
	}

	public List<MdToken> ParseParagraph(string text, int paragraphStart, int paragraphEnd, List<MdToken> escapeTokens)
	{
		this.text = text;
		this.paragraphStart = paragraphStart;
		this.paragraphEnd = paragraphEnd;
		this.escapeTokens = escapeTokens;
		var currentEscapeToken = escapeTokens.FirstOrDefault();
		var result = new List<MdToken>();

		for (var i = paragraphStart; i < paragraphEnd - tag.Open.Length; i++)
		{
			if (CheckEscaping(currentEscapeToken, ref i)) continue;

			if (CheckCompetingTag(ref i)) continue;

			var tagWindow = text.Substring(i, tag.Open.Length);

			if (tagWindow != tag.Open) continue;

			var indexAfterWindow = i + tag.Open.Length;
			if (CheckSymbol(indexAfterWindow, ' '))
			{
				i += tag.Open.Length;
				continue;
			}

			if (!TryParseTag(i, out var endPosition)) continue;

			result.Add(new MdToken(text, i + tag.Open.Length, endPosition, tag.Type));
			i = endPosition;
		}

		return result;
	}

	private bool TryParseTag(int startPosition, out int endPosition)
	{
		endPosition = startPosition;

		if (startPosition >= paragraphEnd - 2 * tag.Close.Length) return false;

		if (CheckSymbol(startPosition - 1, char.IsDigit)) return false;

		var canContainSpaces = startPosition == paragraphStart || text[startPosition - 1] == ' ';

		return TryParseEnding(canContainSpaces, startPosition + tag.Open.Length, out endPosition);
	}

	private bool TryParseEnding(bool canContainSpaces, int startPosition, out int endPosition)
	{
		endPosition = startPosition;
		var containSpaces = false;

		var currentEscapeToken = escapeTokens.FirstOrDefault();

		for (var i = startPosition; i < paragraphEnd - (tag.Close.Length - 1); i++)
		{
			if (CheckEscaping(currentEscapeToken, ref i)) continue;

			var currentSymbol = text[i];

			if (char.IsDigit(currentSymbol)) return false;

			if (currentSymbol == ' ')
			{
				if (!canContainSpaces) return false;

				containSpaces = true;
				continue;
			}

			if (CheckCompetingTag(ref i)) continue;

			var window = text.Substring(i, tag.Close.Length);
			if (window != tag.Close) continue;

			var indexBeforeWindow = i - 1;
			if (CheckSymbol(indexBeforeWindow, ' ')) continue;

			var indexAfterWindow = i + tag.Close.Length;
			if (CheckSymbol(indexAfterWindow, char.IsDigit)) return false;

			if (CheckSymbol(indexAfterWindow, ch => ch != ' ' && containSpaces)) return false;

			endPosition = i;
			return true;
		}

		return false;
	}

	private bool CheckEscaping(MdToken? currentEscapeToken, ref int currentIndex)
	{
		while (currentEscapeToken?.Start < currentIndex) currentEscapeToken = currentEscapeToken?.nextToken as MdToken;

		if (currentEscapeToken?.Start != currentIndex) return false;

		currentIndex = currentEscapeToken.End;
		return true;
	}

	private bool CheckCompetingTag(ref int currentIndex)
	{
		if (!(currentIndex + competingTagPattern?.Length < paragraphEnd)) return false;

		var stopWindow = text.Substring(currentIndex, competingTagPattern.Length);
		if (stopWindow != competingTagPattern) return false;

		currentIndex += competingTagPattern.Length;
		return true;
	}

	private bool CheckSymbol(int symbolIndex, char symbolIs)
	{
		return CheckSymbol(symbolIndex, ch => ch == symbolIs);
	}

	private bool CheckSymbol(int symbolIndex, Func<char, bool> pattern)
	{
		if (symbolIndex <= 0) return false;
		if (symbolIndex >= paragraphEnd) return false;

		var symbol = text[symbolIndex];
		return pattern(symbol);
	}
}