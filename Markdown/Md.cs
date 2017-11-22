using NUnit.Framework;

namespace Markdown
{
	public class Md
	{
		public string RenderToHtml(string markdown)
		{
			string textType = "Text";
			var tokenDescriptions = Initializer.GetTokenDescriptions();

			Parser.Initialize(textType, tokenDescriptions);
			var rawTokens = Parser.Parse(markdown);

			LexicalAnalizer.Initialize(textType, tokenDescriptions);
			var parsedTokens = LexicalAnalizer.Analize(rawTokens);

			TokenAnalizer.Initialize(textType, tokenDescriptions);
			var finalTokens = TokenAnalizer.Analize(parsedTokens);
			return TagRealizer.RealizeTokens(finalTokens);

		}
	}

}