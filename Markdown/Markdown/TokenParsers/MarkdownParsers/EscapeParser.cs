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

			if (CheckEscaping(text, i + escapeTag.Open.Length, paragraphEnd, out var endIndex))
			{
				result.Add(new MdToken(text, i, endIndex - 1, TokenType.Escape));
				i = endIndex;
			}
		}

		return result;
	}

	private bool CheckEscaping(string text, int startIndex, int paragraphEnd, out int endIndex)
	{
		endIndex = startIndex;

		foreach (var tag in tags)
		{
			if (startIndex + tag.Open.Length > paragraphEnd) continue;

			if (text.Substring(startIndex, tag.Open.Length) == tag.Open)
			{
				endIndex = startIndex + tag.Open.Length;
				return true;
			}

			if (tag.Close is null) continue;
			if (startIndex + tag.Close.Length > paragraphEnd) continue;

			if (text.Substring(startIndex, tag.Close.Length) == tag.Close)
			{
				endIndex = startIndex + tag.Close.Length;
				return true;
			}
		}

		return false;
	}
}