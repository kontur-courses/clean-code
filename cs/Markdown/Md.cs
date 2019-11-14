using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
	public static class Md
	{
		public static string Render(string sourceText)
		{
			var availableKeySequences = new FeaturesLoader().AvailableKeySequences;
			var tokenizer = new Tokenizer(availableKeySequences);
			var mainTokenInfo = tokenizer.ParseToTokens(sourceText);
			return HtmlConverter.ConvertToHtml(mainTokenInfo, sourceText);
		}
	}
}