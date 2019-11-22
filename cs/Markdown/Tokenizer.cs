using System.Collections.Generic;
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
				RegisterToken(token);
		}

		private void RegisterToken(IToken token)
		{
			RegisterKeySequence(token.OpeningSequence, token);
			RegisterKeySequence(token.ClosingSequence, token);
			if (!(token is IComplexToken complexToken)) return;
			foreach (var childToken in complexToken.ChildTokens)
			{
				if (childToken.OpeningSequence != token.OpeningSequence)
					RegisterKeySequence(childToken.OpeningSequence, childToken);
				if (childToken.ClosingSequence != token.ClosingSequence)
					RegisterKeySequence(childToken.ClosingSequence, childToken);
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
				contextState.HandleChar(currentIndex);
				UpdateReadingKeySequence(contextState, out var isKeyChar);
				if (!contextState.ReadingKeySequence) 
					continue;
				if (contextState.LastToken.Type is PlainText) 
					contextState.CloseLastPlainText();
				if (HandleKeySequence(contextState))
					continue;
				StopReadingKeySequence(contextState);
				var currentChar = contextState.CurrentChar.ToString();
				if (currentIndex >= sourceText.Length - 1 ||
				    !_supportedKeySequences.TryGetValue(currentChar, out var tokenType) ||
				    !(tokenType is IComplexTokenBlock tokenBlock) ||
				    tokenBlock.ClosingSequence != currentChar) continue;
				contextState.LastToken.PlainText.Clear();
				currentIndex--;
			}
			return contextState.MainToken;
		}

		private static void StopReadingKeySequence(TokenizerContextState contextState)
		{
			var trimLength = 0;
			if (contextState.TokenToClose != null)
			{
				trimLength = contextState.TokenToClose.TokenInfo.Type.ClosingSequence.Length;
				contextState.CloseTokens();
			}
			else if (contextState.TokenToOpen != null)
			{
				contextState.OpenToken();
				trimLength = contextState.TokenToOpen.OpeningSequence.Length;
				contextState.TokenToOpen = null;
			}
			
			if (contextState.CurrentKeySequence.Length >= trimLength)
				contextState.CurrentKeySequence.Remove(0, trimLength);
			contextState.AddCurrentKeySequenceAsPlainText();
			contextState.ReadingKeySequence = false;
		}

		private void UpdateReadingKeySequence(TokenizerContextState contextState, out bool isKeyChar)
		{
			isKeyChar = _supportedKeyChars.Contains(contextState.CurrentChar) &&
			                !contextState.ReadingAsPlainText;
			if (contextState.ReadingKeySequence) return;
			if (isKeyChar && contextState.Shielded)
			{
				isKeyChar = false;
				RemoveLastPlainTextChar(contextState);
				contextState.Shielded = false;
			}

			contextState.ReadingKeySequence = isKeyChar;
			if (isKeyChar) return;
			contextState.AddCurrentKeySequenceAsPlainText();
			contextState.CurrentKeySequence.Clear();
		}
		
		private static void RemoveLastPlainTextChar(TokenizerContextState contextState)
		{
			var lastCharIndex = contextState.LastToken.PlainText.Length - 1;
			contextState.LastToken.PlainText.Remove(lastCharIndex, 1);
		}

		private bool HandleKeySequence(TokenizerContextState contextState)
		{
			var isKeySequence = _supportedKeySequences
				.TryGetValue(contextState.CurrentKeySequence.ToString(), out var tokenType);
			if (isKeySequence)
			{
				contextState.TryCloseToken();
				contextState.TryOpenToken(tokenType);
			}
			
			if (contextState.CurrentIndex + 1 == contextState.SourceText.Length || 
			    tokenType is IComplexToken || tokenType is IComplexTokenBlock)
				StopReadingKeySequence(contextState);
			return isKeySequence;
		}
	}
}