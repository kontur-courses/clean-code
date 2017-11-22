using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
	public static class Parser
	{
		private static string TextType { get; set; }
		private static TokenDescription[] TokensDescriptions { get; set; }

		public static void Initialize(string textTypeIn, TokenDescription[] tokenDescriptionsIn)
		{
			TextType = textTypeIn;
			TokensDescriptions = tokenDescriptionsIn;
		}

		public static RawToken[] Parse(string input)
		{
			var parsingResult = new List<RawToken>();
			var patternsLenghts = TokensDescriptions.Where(td => !string.IsNullOrEmpty(td.pattern)).Select(td => td.pattern.Length).OrderByDescending(x => x).ToArray();

			var previousTolkenStart = 0;
			for (int i = 0; i < input.Length; i++)
			{
				foreach (var j in patternsLenghts)
				{
					if (i + j > input.Length)
						continue;

					var matchToken = TokensDescriptions.FirstOrDefault(td => td.pattern == input.Substring(i, j));

					if (matchToken != null)
					{
						var tokenValue = input.Substring(previousTolkenStart, i - previousTolkenStart);
						if (previousTolkenStart < i)
							parsingResult.Add(new RawToken(TextType, tokenValue));
						parsingResult.Add(new RawToken(matchToken.Type));
						previousTolkenStart = i + j;
						i += j-1;
						break;
					}
				}
			}

			return parsingResult.ToArray();
		}

	}
}

