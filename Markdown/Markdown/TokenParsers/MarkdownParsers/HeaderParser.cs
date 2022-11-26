using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenParsers.MarkdownParsers;

public class HeaderParser : IMarkdownTagParser
{
	public HeaderParser(MarkdownTag tag)
	{
		Tag = tag;
	}

	public MarkdownTag Tag { get; }

	public List<MdToken> ParseParagraph(string text, int paragraphStart, int paragraphEnd, List<MdToken> escapeTokens)
	{
		var emptyList = new List<MdToken>();

		if (escapeTokens.FirstOrDefault()?.Start == 0) return emptyList;
		if (paragraphStart == text.Length - 1) return emptyList;
		if (paragraphEnd - paragraphStart < Tag.Open.Length) return emptyList;
		if (text.Substring(paragraphStart, Tag.Open.Length) != Tag.Open) return emptyList;

		var result = new List<MdToken> { new(text, paragraphStart + Tag.Open.Length, paragraphEnd, TokenType.Header) };
		return result;
	}
}