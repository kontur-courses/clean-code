using Markdown.Tokens;

namespace Markdown.TokenParsers.MarkdownParsers;

public interface IMarkdownTagParser
{
	public static char EscapeSymbol => '/';

	public List<MdToken> ParseParagraph(string text, int paragraphStart, int paragraphEnd, List<MdToken> escapeTokens);
}