using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
	public static class TagRealizer
	{
		private static string TextType { get; set; }
		private static TokenDescription[] TokensDescriptions { get; set; }


		public static void Initialize(string textTypeIn, TokenDescription[] tokenDescriptionsIn)
		{
			TextType = textTypeIn;
			TokensDescriptions = tokenDescriptionsIn;
		}

		public static string RealizeTokens(Token[] tokens)
		{
			var result = new StringBuilder();
			foreach (var currentToken in tokens)
			{
				if (currentToken.Type == TextType)
				{
					result.Append(currentToken.Value);
					continue;
				}
				var token = currentToken;
				var currentTd = TokensDescriptions.Single(td => td.Type == token.Type);
				var valueToAppend = currentToken.TagType == TagType.Opening ? currentTd.OpeningTag : currentTd.ClosingTag;
				result.Append(valueToAppend);
			}
			return result.ToString();
		}
	}
}
