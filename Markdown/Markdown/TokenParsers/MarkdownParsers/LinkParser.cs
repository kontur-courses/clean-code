using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenParsers.MarkdownParsers;

internal class LinkParser : IMarkdownTagParser
{
	public MarkdownTag Tag => new MarkdownTag("(", ")", TokenType.Link);

	private List<MdToken> escapeTokens;

	public List<MdToken> ParseParagraph(string text, int paragraphStart, int paragraphEnd, List<MdToken> escapeTokens)
	{
		throw new NotImplementedException();
		//this.escapeTokens = escapeTokens;
		//var currentEscapeToken = escapeTokens.FirstOrDefault();

		//for (var i = 0; i < paragraphEnd; i++)
		//{
		//	if(CheckEscaping(currentEscapeToken, ref i)) continue;

		//	var symbol = text[i];
		//	if($"{symbol}" != Tag.Open) continue;


		//}
	}

	private bool CheckEscaping(MdToken? currentEscapeToken, ref int currentIndex)
	{
		while (currentEscapeToken?.Start < currentIndex) currentEscapeToken = currentEscapeToken?.nextToken as MdToken;

		if (currentEscapeToken?.Start != currentIndex) return false;

		currentIndex = currentEscapeToken.End;
		return true;
	}
}