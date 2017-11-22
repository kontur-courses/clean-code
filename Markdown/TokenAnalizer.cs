using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;

namespace Markdown
{
	public static class TokenAnalizer
	{
		private static string TextType { get; set; }
		private static TokenDescription[] TokensDescriptions { get; set; }

		public static void Initialize(string textTypeIn, TokenDescription[] tokenDescriptionsIn)
		{
			TextType = textTypeIn;
			TokensDescriptions = tokenDescriptionsIn;
		}

		public static Token[] Analize(Token[] parsedTokens)
		{
			var result = new List<Token>();

			var tokenStack = new Stack<Token>();
			var openedTags = TokensDescriptions.ToDictionary(td => td.Type, td => false);

			StringBuilder tokenToAddText = null;
			for (int i = 0; i < parsedTokens.Length; i++)
			{
				var nextToken = i + 1 < parsedTokens.Length ? parsedTokens[i + 1] : null;
				var currentToken = parsedTokens[i];

				if (currentToken.Type == TextType)
				{
					var newTokenToAddText = HandleTextToken(currentToken, tokenToAddText);
					tokenToAddText = newTokenToAddText;
					continue;
				}


				if (!ValidateTokenTagType(currentToken, openedTags))
				{
					var currentTokenValue = TokensDescriptions.Single(td => td.Type == currentToken.Type).pattern;
					if (tokenToAddText == null)
						tokenToAddText = new StringBuilder(currentTokenValue);
					else
						tokenToAddText.Append(currentTokenValue);
					continue;
				}

				if (tokenToAddText != null)
				{
					result.Add(new Token(TextType, TagType.Undefined, tokenToAddText.ToString()));
					tokenToAddText = null;
				}

				if (currentToken.TagType == TagType.Opening)
				{
					tokenStack.Push(currentToken);
					openedTags[currentToken.Type] = true;
				}

				if (currentToken.TagType == TagType.Closing)
				{
					if (tokenStack.Peek().Type != currentToken.Type && tokenStack.Peek().Type == nextToken.Type && TrySwitch(currentToken, nextToken))
					{
						parsedTokens[i] = nextToken;
						parsedTokens[i + 1] = currentToken;
						i--;
						continue;
					}
					tokenStack.Pop();
					openedTags[currentToken.Type] = false;
				}
				result.Add(currentToken);
			}
			return result.ToArray();
		}


		private static bool TrySwitch(Token currentToken, Token nextToken)
		{
			if (nextToken.Type == TextType)
				return false;
			var currentTokenValue = TokensDescriptions.Single(td => td.Type == currentToken.Type).pattern;
			var nexTokenValue = TokensDescriptions.Single(td => td.Type == nextToken.Type).pattern;
			return currentTokenValue + nexTokenValue == nexTokenValue + currentTokenValue;
		}


		private static StringBuilder HandleTextToken(Token currentToken, StringBuilder tokenToAddText)
		{
			if (tokenToAddText == null)
				tokenToAddText = new StringBuilder(currentToken.Value);
			else
				tokenToAddText.Append(currentToken.Value);
			return tokenToAddText;
		}

		private static bool ValidateTokenTagType(Token currentToken, Dictionary<string, bool> openedTags)
		{
			if (currentToken.TagType == TagType.Closing && openedTags[currentToken.Type])
				return true;
			if (currentToken.TagType == TagType.Opening && !openedTags[currentToken.Type])
				return true;
			return false;
		}


		private static bool ValidateTokenTagType(Token tokenToValidate, bool previousSameTokenTypeTagIndex)
		{
			return ((previousSameTokenTypeTagIndex && tokenToValidate.TagType == TagType.Closing) ||
					 (!previousSameTokenTypeTagIndex && tokenToValidate.TagType == TagType.Opening));
		}

	}
}
