using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenParsers.MarkdownParsers;

public class EscapeParser : IMarkdownTagParser
{
	private readonly MarkdownTag escapeTag;
	private readonly IEnumerable<MarkdownTag> tags;

	public EscapeParser(MarkdownTag escapeTag, IEnumerable<MarkdownTag> tags)
	{
		this.escapeTag = escapeTag;
		this.tags = tags;
	}

	public List<MdToken> ParseParagraph(string text, int paragraphStart, int paragraphEnd,
		List<MdToken> escapeTokens = null)
	{
		var result = new List<MdToken>();

		for (var i = paragraphStart; i < paragraphEnd; i++)
		{
			if (text.Substring(i, escapeTag.Open.Length) != escapeTag.Open) continue;

			if (!CheckEscaping(text, i + escapeTag.Open.Length, paragraphEnd, out var endIndex)) continue;

			result.Add(new MdToken(text, i, endIndex - 1, TokenType.Escape));
			i = endIndex;
		}

		return result;
	}

	private bool CheckEscaping(string text, int startIndex, int paragraphEnd, out int endIndex)
	{
		endIndex = startIndex;

		foreach (var tag in tags)
		{
			if (CheckTagPart(text, paragraphEnd, startIndex, tag.Open, out endIndex)) return true;

			if (tag.Close is null) continue;

			return CheckTagPart(text, paragraphEnd, startIndex, tag.Close, out endIndex);
		}

		return false;
	}

	private bool CheckTagPart(string text, int paragraphEnd, int startIndex, string partPattern, out int endIndex)
	{
		endIndex = startIndex;

		if (startIndex + partPattern.Length > paragraphEnd) return false;

		if (text.Substring(startIndex, partPattern.Length) != partPattern) return false;

		endIndex = startIndex + partPattern.Length;
		return true;
	}
}