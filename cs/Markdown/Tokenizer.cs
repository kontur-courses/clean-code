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
			var contextState = new TokenizerContextState(sourceText);
			for (var currentIndex = 0; currentIndex < sourceText.Length; currentIndex++)
			{
				contextState.Update(currentIndex);
				if (!contextState.ReadingKeySequence)
				{
					var isKeyChar = _supportedKeyChars.Contains(contextState.CurrentChar);
					if (isKeyChar && contextState.Shielded)
					{
						isKeyChar = false;
						RemoveLastPlainTextChar(contextState);
						contextState.Shielded = false;
					}
					contextState.ReadingKeySequence = isKeyChar;
					if (!isKeyChar)
					{
						contextState.AddCurrentKeySequenceAsPlainText();
						contextState.CurrentKeySequence.Clear();
					}
				}
				if (!contextState.ReadingKeySequence) continue;
				if (contextState.LastToken.Type is PlainText) contextState.CloseLastToken();
				var isKeySequence = CheckIsKeySequence(contextState, out var tokenType);
				if (isKeySequence)
				{
					if (contextState.TryCloseToken())
					{
						contextState.ReadingKeySequence = false;
						continue;
					}
					if (tokenType.IsOpeningKeySequence(contextState))
					{
						if (contextState.LastToken.Type is PlainText || contextState.LastToken.InnerTokens.Count > 0)
							contextState.AddChildToken(tokenType);
						else
							contextState.LastToken.Type = tokenType;
						continue;
					}
				}
				if (currentIndex == sourceText.Length - 1)
				{
					if (contextState.LastToken.InnerTokens.Count == 0)
						contextState.CurrentKeySequence.Remove(0, contextState.LastToken.Type.OpeningSequence.Length);
					contextState.AddCurrentKeySequenceAsPlainText();
					continue;
				}
				if (_supportedKeyChars.Contains(contextState.CurrentChar))
					continue;
				contextState.CurrentKeySequence.Remove(0, contextState.LastToken.Type.OpeningSequence.Length);
				contextState.AddCurrentKeySequenceAsPlainText();
				contextState.ReadingKeySequence = false;
			}
			return contextState.MainToken;
		}

		private static void RemoveLastPlainTextChar(TokenizerContextState contextState)
		{
			var lastCharIndex = contextState.LastToken.PlainText.Length - 1;
			contextState.LastToken.PlainText.Remove(lastCharIndex, 1);
		}

		private bool CheckIsKeySequence(TokenizerContextState contextState, out IToken tokenType)
		{
			if (_supportedKeySequences.TryGetValue(contextState.CurrentKeySequence.ToString(), out tokenType))
				return true;
			tokenType = new PlainText();
			return false;
		}
	}
}