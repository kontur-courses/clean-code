﻿using Markdown.Tokens;

namespace Markdown.TokenParsers.MarkdownParsers;

public class HeaderParser : IMarkdownTagParser
{
	public List<MdToken> ParseParagraph(string text, int paragraphStart, int paragraphEnd, List<MdToken> escapeTokens)
	{
		var emptyList = new List<MdToken>();

		if (escapeTokens.FirstOrDefault()?.Start == 0) return emptyList;
		if (paragraphStart == text.Length - 1) return emptyList;
		if (paragraphEnd - paragraphStart < 3) return emptyList;
		if (text[paragraphStart] != '#' && text[paragraphStart + 1] != ' ') return emptyList;

		var result = new List<MdToken> { new(text, paragraphStart + 2, paragraphEnd, TokenType.Header) };
		return result;
	}
}