using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenParsers.MarkdownParsers;

internal class LinkParser : IMarkdownTagParser
{
	private readonly MarkdownTag linkDescriptionTag;

	private readonly MarkdownTag linkTag;
	private MdToken currentEscapeToken;
	private int paragraphEnd;
	private string text;

	public LinkParser(MarkdownTag containerTag, MarkdownTag linkTag, MarkdownTag linkDescriptionTag)
	{
		Tag = containerTag;
		this.linkTag = linkTag;
		this.linkDescriptionTag = linkDescriptionTag;
	}

	public MarkdownTag Tag { get; }

	public List<MdToken> ParseParagraph(string text, int paragraphStart, int paragraphEnd, List<MdToken> escapeTokens)
	{
		this.text = text;
		this.paragraphEnd = paragraphEnd;
		currentEscapeToken = escapeTokens.FirstOrDefault();
		var result = new List<MdToken>();

		var index = paragraphStart;
		while (index < paragraphEnd)
		{
			if (TryParseLink(index, out var endIndex, out var containerToken))
			{
				result.Add(containerToken);
				index = endIndex;
			}

			index++;
		}

		return result;
	}

	private bool TryParseLink(int startIndex, out int endIndex, out MdToken containerToken)
	{
		containerToken = new MdToken(null, 0, 0, TokenType.Container);
		endIndex = startIndex;

		if (CheckEscaping(currentEscapeToken, ref startIndex)) return false;

		if (!TryParseToken(startIndex, linkDescriptionTag, out endIndex, out var parsedLinkDescription)) return false;
		startIndex = endIndex + 1;
		if (!TryParseToken(startIndex, linkTag, out endIndex, out var parsedLink)) return false;

		if (parsedLinkDescription.End + linkTag.Open.Length + 1 != parsedLink.Start) return false;

		parsedLink.nextToken = parsedLinkDescription;
		containerToken = new MdToken(
			text,
			parsedLinkDescription.Start - linkDescriptionTag.Open.Length,
			parsedLink.End + linkTag.Close?.Length ?? 0,
			TokenType.Container)
		{
			nestingTokens = parsedLink
		};

		return true;
	}

	private bool TryParseToken(int startIndex, MarkdownTag tag, out int endIndex, out MdToken parsedToken)
	{
		parsedToken = null;
		endIndex = startIndex;

		if (CheckEscaping(currentEscapeToken, ref startIndex)) return false;

		if (startIndex + tag.Open.Length >= paragraphEnd) return false;

		var window = text.Substring(startIndex, tag.Open.Length);
		if (window != tag.Open) return false;

		for (var i = startIndex + tag.Open.Length; i <= paragraphEnd - tag.Close?.Length; i++)
		{
			if (CheckEscaping(currentEscapeToken, ref i)) continue;

			if (text[i] == ' ') return false;

			window = text.Substring(i, tag.Close.Length);

			if (window != tag.Close) continue;

			endIndex = i;
			parsedToken = new MdToken(text, startIndex + tag.Open.Length, i, tag.Type);
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
}