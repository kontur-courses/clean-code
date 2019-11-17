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
			ConvertToHtml(mainToken, htmlText, sourceText);
			return htmlText.ToString();
		}

		private static void ConvertToHtml(TokenInfo currentToken, StringBuilder htmlText, string sourceText)
		{
			if (!(currentToken.Closed || currentToken.Type is PlainText))
			{
				currentToken.PlainText.Append(currentToken.Type.OpeningSequence);
				currentToken.Type = new PlainText();
			}
			if (currentToken.Type is PlainText)
				htmlText.Append(currentToken.PlainText);
			if (currentToken.InnerTokens.Count == 0)
				return;
			
			htmlText.Append(currentToken.Type.GetHtmlOpeningTag(currentToken, sourceText));
			foreach (var innerToken in currentToken.InnerTokens)
				ConvertToHtml(innerToken, htmlText, sourceText);
			htmlText.Append(currentToken.Type.GetHtmlClosingTag(currentToken, sourceText));
		}
	}
}