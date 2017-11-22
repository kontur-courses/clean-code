using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
	public static class LexicalAnalizer
	{
		private static string TextType { get; set; }
		private static TokenDescription[] TokensDescriptions { get; set; }

		public static void Initialize(string textTypeIn, TokenDescription[] tokenDescriptionsIn)
		{
			TextType = textTypeIn;
			TokensDescriptions = tokenDescriptionsIn;
		}

		public static Token[] Analize(RawToken[] parsedRawTokens)
		{
			var result = new List<Token>();
			RawToken previousParsedToken = null;
			StringBuilder textTokenValueToAdd = null;
			for (int i = 0; i < parsedRawTokens.Length; i++)
			{
				if (i > 0)
					previousParsedToken = parsedRawTokens[i - 1];
				var currentParsedToken = parsedRawTokens[i];
				var nextParsedToken = i + 1 < parsedRawTokens.Length ? parsedRawTokens[i + 1] : null;


				var currentTokentagType = GetTagType(previousParsedToken, currentParsedToken, nextParsedToken);
				if (currentTokentagType == TagType.Undefined)
				{
					if (textTokenValueToAdd == null)
						textTokenValueToAdd = new StringBuilder(GetTokenValue(currentParsedToken));
					else
						textTokenValueToAdd.Append(GetTokenValue(currentParsedToken));
					continue;
				}

				if (textTokenValueToAdd != null)
				{
					result.Add(new Token(TextType, TagType.Undefined, textTokenValueToAdd.ToString()));
					textTokenValueToAdd = null;
				}
				result.Add(new Token(currentParsedToken.Type, currentTokentagType));

			}

			return result.ToArray();
		}

		private static TagType GetTagType(RawToken previousToken, RawToken currentToken, RawToken nextToken)
		{
			if (currentToken.Type == TextType)
				return TagType.Undefined;
			var previousSymbol = GetTokenProperChar(previousToken, str => str[str.Length-1]);
			var nextSymbol = GetTokenProperChar(nextToken, str => str[0]);
			var tagTypeDeterminant = TokensDescriptions.Single(td => td.Type == currentToken.Type).TagTypeDeterminant;
			return tagTypeDeterminant(previousSymbol, nextSymbol);
		}

		private static char? GetTokenProperChar(RawToken token, Func<string, char> selectProperChar)
		{
			if (token == null)
				return null;
			var value = GetTokenValue(token);
			return
				selectProperChar(value);
		}

		private static string GetTokenValue(RawToken token)
		{
			if (token.Type == TextType)
				return token.Value;
			
			return TokensDescriptions.Single(td => td.Type == token.Type).pattern;
		}
	}
}
