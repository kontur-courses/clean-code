using Markdown.Tokens;

namespace Markdown.TokenParsers.MarkdownParsers;

public class ItalicParser : IMarkdownTagParser
{
	private List<MdToken> escapeTokens;
	private int paragraphEnd;
	private int paragraphStart;
	private string text;

	public List<MdToken> ParseParagraph(string text, int paragraphStart, int paragraphEnd, List<MdToken> escapeTokens)
	{
		this.text = text;
		this.paragraphStart = paragraphStart;
		this.paragraphEnd = paragraphEnd;
		this.escapeTokens = escapeTokens;
		var currentEscapeToken = escapeTokens.FirstOrDefault();
		var result = new List<MdToken>();

		for (var i = paragraphStart; i < paragraphEnd - 1; i++)
		{
			while (currentEscapeToken?.Start < i) currentEscapeToken = currentEscapeToken?.nextToken as MdToken;

			if (currentEscapeToken?.Start == i)
			{
				i = currentEscapeToken.End;
				continue;
			}

			if (text[i] == '_')
			{
				if (text[i + 1] == '_' || text[i + 1] == ' ')
				{
					i++;
					continue;
				}

				var isParsed = TryParseTag(i, out var endPosition);
				if (!isParsed) continue;

				result.Add(new MdToken(text, i + 1, endPosition, TokenType.Italic));
				i = endPosition;
			}
		}

		return result;
	}

	private bool TryParseTag(int startPosition, out int endPosition)
	{
		endPosition = startPosition;

		if (startPosition >= paragraphEnd - 2) return false;

		if (startPosition > 0 && char.IsDigit(text[startPosition - 1])) return false;

		if (startPosition > 0)
			if (startPosition == 1 && text[startPosition - 1] == IMarkdownTagParser.EscapeSymbol)
				return false;

		var canContainSpaces = startPosition == paragraphStart || text[startPosition - 1] == ' ';
		return TryParseEnding(canContainSpaces, startPosition + 1, out endPosition);
	}

	private bool TryParseEnding(bool canContainSpaces, int startPosition, out int endPosition)
	{
		endPosition = startPosition;
		var containSpaces = false;

		var currentEscapeToken = escapeTokens.FirstOrDefault();

		for (var i = startPosition + 1; i < paragraphEnd; i++)
		{
			while (currentEscapeToken?.Start < i) currentEscapeToken = currentEscapeToken?.nextToken as MdToken;

			if (currentEscapeToken?.Start == i)
			{
				i = currentEscapeToken.End;
				continue;
			}

			var symbol = text[i];

			if (char.IsDigit(symbol)) return false;

			if (symbol == ' ')
			{
				if (!canContainSpaces) return false;

				containSpaces = true;
				continue;
			}

			if (symbol != '_') continue;

			if (text[i - 1] == ' ') continue;

			if (i == paragraphEnd - 1)
			{
				endPosition = i;
				return true;
			}

			var nextSymbol = text[i + 1];
			if (nextSymbol == '_' || text[i - 1] == '_')
			{
				i++;
				continue;
			}

			if (char.IsDigit(nextSymbol)) return false;

			if (nextSymbol != ' ' && containSpaces) return false;

			endPosition = i;
			return true;
		}

		return false;
	}
}