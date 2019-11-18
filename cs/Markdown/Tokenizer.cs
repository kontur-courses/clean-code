using System.Collections.Generic;

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
			var contextState = new TokenizerContextState(sourceText, _supportedKeyChars, _supportedKeySequences);
			for (var currentIndex = 0; currentIndex < sourceText.Length; currentIndex++)
				contextState.Update(currentIndex);
			return contextState.MainToken;
		}
	}
}