using System.Collections.Generic;
using Markdown.Features;

namespace Markdown
{
	internal class Tokenizer
	{
		private readonly Dictionary<string, IToken> _supportedKeySequences;
		private readonly HashSet<string> _allKeyParts;

		public Tokenizer(IEnumerable<IToken> supportedTokens)
		{
			_supportedKeySequences = new Dictionary<string, IToken>();
			_allKeyParts = new HashSet<string>();
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
			var currentSequence = "";
			foreach (var keyChar in keySequence)
			{
				currentSequence += keyChar;
				_allKeyParts.Add(currentSequence);
			}
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
				if (HandleKeySequence(contextState, out var tokenType))
					continue;
				if ((currentIndex + 1 < sourceText.Length || !isKeyChar) &&
					(contextState.TokenToOpen != null || contextState.TokenToClose != null) &&
				    !(tokenType is IComplexToken) && !(tokenType is IComplexTokenBlock))
					currentIndex--;
				StopReadingKeySequence(contextState);
			}
			return contextState.MainToken;
		}

		private static void StopReadingKeySequence(TokenizerContextState contextState)
		{
			if (contextState.TokenToClose != null)
				contextState.CloseTokens();
			else if (contextState.TokenToOpen != null)
			{
				contextState.OpenToken();
				contextState.TokenToOpen = null;
			}
			else			
				contextState.AddCurrentKeySequenceAsPlainText();
			contextState.ReadingKeySequence = false;
		}

		private void UpdateReadingKeySequence(TokenizerContextState contextState, out bool isKeyChar)
		{
			isKeyChar = _allKeyParts.Contains(contextState.CurrentKeySequence.ToString()) &&
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

		private bool HandleKeySequence(TokenizerContextState contextState, out IToken tokenType)
		{
			var isKeySequence = _supportedKeySequences
				.TryGetValue(contextState.CurrentKeySequence.ToString(), out tokenType);
			if (isKeySequence)
			{
				contextState.TryCloseToken();
				contextState.TryOpenToken(tokenType);
			}

			if (contextState.CurrentIndex + 1 == contextState.SourceText.Length ||
			    tokenType is IComplexToken || tokenType is IComplexTokenBlock)
				isKeySequence = false;
			return isKeySequence;
		}
	}
}