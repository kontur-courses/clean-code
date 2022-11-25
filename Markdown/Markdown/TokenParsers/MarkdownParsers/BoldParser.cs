﻿using Markdown.Tokens;

namespace Markdown.TokenParsers.MarkdownParsers;

public class BoldParser : IMarkdownTagParser
{
	private int paragraphEnd;
	private int paragraphStart;
	private string text;

	public static char EscapeSymbol => '/';

	public List<MdToken> ParseParagraph(string text, int paragraphStart, int paragraphEnd)
	{
		this.text = text;
		this.paragraphStart = paragraphStart;
		this.paragraphEnd = paragraphEnd;
		var result = new List<MdToken>();

		for (var i = paragraphStart; i < paragraphEnd - 2; i++)
			if (text[i] == '_' && text[i + 1] == '_')
			{
				if (text[i + 2] == ' ')
				{
					i += 2;
					continue;
				}

				var isParsed = TryParseTag(i, out var endPosition);
				if (!isParsed) continue;

				result.Add(new MdToken(text, i + 2, endPosition, TokenType.Bold));
				i = endPosition;
			}

		return result;
	}

	private bool TryParseTag(int startPosition, out int endPosition)
	{
		endPosition = startPosition;

		if (startPosition >= paragraphEnd - 4) return false;

		if (startPosition > 0 && char.IsDigit(text[startPosition - 1])) return false;

		var canContainSpaces = startPosition == paragraphStart || text[startPosition - 1] == ' ';
		return TryParseEnding(canContainSpaces, startPosition + 2, out endPosition);
	}

	private bool TryParseEnding(bool canContainSpaces, int startPosition, out int endPosition)
	{
		endPosition = startPosition;
		var containSpaces = false;

		for (var i = startPosition + 1; i < paragraphEnd - 1; i++)
		{
			var symbol = text[i];

			if (char.IsDigit(symbol)) return false;

			if (symbol == ' ')
			{
				if (!canContainSpaces) return false;

				containSpaces = true;
				continue;
			}

			var nextSymbol = text[i + 1];
			if (symbol != '_' || nextSymbol != '_') continue;

			if (text[i - 1] == ' ') continue;

			if (i == paragraphEnd - 2)
			{
				endPosition = i;
				return true;
			}

			var nextNextSymbol = text[i + 2];

			if (char.IsDigit(nextNextSymbol)) return false;

			if (nextNextSymbol != ' ' && containSpaces) return false;

			endPosition = i;
			return true;
		}

		return false;
	}
}