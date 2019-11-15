using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Features;

namespace Markdown
{
	internal class Tokenizer
	{
		private readonly Dictionary<string, IToken> _supportedKeySequences;
		private readonly HashSet<char> _supportedKeyChars;

		public Tokenizer(IEnumerable<IToken> supportedTokens)
		{
			_supportedKeySequences = new Dictionary<string, IToken>();
			_supportedKeyChars = new HashSet<char>();
			foreach (var token in supportedTokens)
			{
				RegisterKeySequence(token.OpeningSequence, token);
				RegisterKeySequence(token.ClosingSequence, token);
			}
		}

		private void RegisterKeySequence(string keySequence, IToken token)
		{
			foreach (var keyChar in keySequence)
				_supportedKeyChars.Add(keyChar);
			_supportedKeySequences[keySequence] = token;	
		}
		
		public TokenInfo ParseToTokens(string sourceText)
		{
			if (string.IsNullOrEmpty(sourceText))
				return new TokenInfo(0, new PlainText());
			
			var contextState = new TokenizerContextState(sourceText);
			for (var currentIndex = 0; currentIndex < sourceText.Length; currentIndex++)
			{
				contextState.Update(currentIndex);
				if (!contextState.ReadingKeySequence)
				{
					var isKeyChar = CheckIsCharInKeySequence(contextState, out var tokenType);
					if (isKeyChar && !contextState.Shielded)
					{
						contextState.CurrentKeySequence.Append(contextState.CurrentChar);
						if (tokenType.IsOpeningKeySequence(contextState))
							contextState.AddChildToken(tokenType);
					}
					else if (!contextState.Shielded)
						contextState.AddCurrentCharAsPlainText();
					else
					{
						var plainTextLength = contextState.LastToken.PlainText.Length;
						contextState.LastToken.PlainText.Remove(plainTextLength - 1, 1);
					}
				}
				if (contextState.ReadingKeySequence)
				{
					if (contextState.TryCloseToken())
						continue;
					contextState.CurrentKeySequence.Append(contextState.CurrentChar);
					var currentSequence = contextState.CurrentKeySequence.ToString();
					if (_supportedKeySequences.TryGetValue(currentSequence, out var tokenType))
					{
						contextState.LastToken.Type = tokenType;
						contextState.LastToken.PlainText.Clear();
						contextState.LastToken.PlainText.Append(contextState.CurrentKeySequence);
					}
					else
					{
						contextState.CurrentKeySequence.Clear();
						contextState.AddCurrentCharAsPlainText();
					}
				}
			}

			return contextState.MainToken;
		}

		private bool CheckIsCharInKeySequence(TokenizerContextState contextState, out IToken tokenType)
		{
			if (_supportedKeySequences.TryGetValue(contextState.CurrentChar, out tokenType)) return true;
			if (!_supportedKeyChars.Contains(char.Parse(contextState.CurrentChar))) return false;
			tokenType = new PlainText();
			return true;
		}
	}
}