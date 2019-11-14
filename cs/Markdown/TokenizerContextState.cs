using System;
using System.Text;

namespace Markdown
{
	internal class TokenizerContextState
	{
		private TokenContainer _mainToken;
		private TokenContainer _lastToken;

		private StringBuilder _currentKeySequence;
		private bool _readingKeySequence;
		private bool _inPlainTextToken;

		public bool ShieldedChar { get; set; }
		public string SourceText { get; }
		public bool ReadAsPlainText { get; set; }
		public int CurrentIndex { get; private set; }
		public char CurrentChar { get; private set; }

		public TokenizerContextState(string sourceText)
		{
		}

		public char Update(int currentIndex, char currentChar)
		{
			throw new NotImplementedException();
		}

		private IToken RecognizeKeySequence()
		{
			throw new NotImplementedException();
		}

		public void AddChildToken(TokenInfo newTokenInfo)
		{
		}

		public bool TryCloseToken(int currentIndex, string closingKeySequence)
		{
			throw new NotImplementedException();
		}

		public void AddAsPlainText(char newChar)
		{
		}
	}

	internal class TokenContainer
	{
		public TokenContainer _parentToken;
		public TokenInfo TokenInfo;
	}
}