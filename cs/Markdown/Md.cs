using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
	public static class Md
	{
		public static string Render(string sourceText)
		{
			var supportedTokens = new FeaturesLoader().SupportedTokens;
			var tokenizer = new Tokenizer(supportedTokens);
			var mainTokenInfo = tokenizer.ParseToTokens(sourceText);
			return HtmlConverter.ConvertToHtml(mainTokenInfo, sourceText);
		}
	}
}