using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Markdown.Features;

namespace Markdown
{
	internal class HtmlConverter
	{
		public static string ConvertToHtml(TokenInfo mainToken, string sourceText)
		{
			var htmlText = new StringBuilder();
			ParseTokens(mainToken, htmlText, sourceText);
			return htmlText.ToString();
		}

		private static void ParseTokens(TokenInfo currentToken, StringBuilder htmlText, string sourceText)
		{
			if (!currentToken.Closed)
				currentToken.TokenType = new PlainText();
			if (currentToken.TokenType is PlainText)
				htmlText.Append(currentToken.PlainText);
			if (currentToken.InnerTokens.Count == 0)
				return;
			
			htmlText.Append(currentToken.TokenType.GetHtmlOpeningTag(currentToken, sourceText));
			foreach (var innerToken in currentToken.InnerTokens)
				ParseTokens(innerToken, htmlText, sourceText);
			htmlText.Append(currentToken.TokenType.GetHtmlClosingTag(currentToken, sourceText));
		}
	}
}