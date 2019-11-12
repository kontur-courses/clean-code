using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
	public static class Md
	{
		public static string Render(string sourceText)
		{
			FeaturesLoader.LoadFeatures();
			var mainTokenInfo = ParseToTokens(sourceText);
			return HtmlConverter.ConvertToHtml(mainTokenInfo, sourceText);
		}

		private static TokenInfo ParseToTokens(string sourceText)
		{
			var context = new Context(sourceText);
			throw new NotImplementedException();
		}
	}
}