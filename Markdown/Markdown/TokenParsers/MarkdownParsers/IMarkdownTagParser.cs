using Markdown.Tokens;

namespace Markdown.TokenParsers.MarkdownParsers;

public interface IMarkdownTagParser
{
	public List<MdToken> ParseParagraph(string text, int paragraphStart, int paragraphEnd);
}