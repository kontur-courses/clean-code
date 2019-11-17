using System.Text;
using Markdown.Features;

namespace Markdown
{
	internal class TokenizerContextState
	{
		private TokenContainer _mainToken;
		private TokenContainer _lastToken;

		private bool _readingKeySequence;
		public bool ReadingKeySequence
		{
			get => _readingKeySequence;
			set
			{
				if (!value && _readingKeySequence)
					CurrentKeySequence.Clear();
				_readingKeySequence = value;
			}
		}
		public TokenInfo MainToken => _mainToken.TokenInfo;
		public TokenInfo LastToken => _lastToken.TokenInfo;
		public StringBuilder CurrentKeySequence { get; } = new StringBuilder();
		public bool Shielded { get; set; }
		public string SourceText { get; }
		public int CurrentIndex { get; private set; }
		public char CurrentChar { get; private set; }

		public TokenizerContextState(string sourceText)
		{
			SourceText = sourceText;
			var mainToken = new TokenInfo(0, new PlainText());
			_mainToken = new TokenContainer(mainToken);
			_lastToken = _mainToken;
		}

		private class TokenContainer
		{
			public TokenContainer ParentToken { get; }
			public TokenInfo TokenInfo { get; }

			public TokenContainer(TokenInfo tokenInfo, TokenContainer parent=null)
			{
				TokenInfo = tokenInfo;
				ParentToken = parent;
			}
		}

		public void Update(int currentIndex)
		{
			CurrentIndex = currentIndex;
			CurrentChar = SourceText[currentIndex];
			Shielded = Shielded || CurrentChar == '\\';
			CurrentKeySequence.Append(CurrentChar);
			ReadingKeySequence = _readingKeySequence && !Shielded;
		}

		public bool TryCloseToken()
		{
			var currentToken = _lastToken;
			while (currentToken != null && currentToken != _mainToken)
			{
				if (!(currentToken.TokenInfo.Type is PlainText) && 
				    currentToken.TokenInfo.TryClose(this))
				{
					RemoveRedundantToken();
					_lastToken = currentToken.ParentToken;
					return true;
				}
				currentToken = currentToken.ParentToken;
			}
			return false;
		}

		private void RemoveRedundantToken()
		{
			if (LastToken.InnerTokens.Count != 0 || LastToken.Closed || LastToken.Type is PlainText) return;
			var extraTokenParent = _lastToken.ParentToken.TokenInfo;
			extraTokenParent.InnerTokens.RemoveAt(extraTokenParent.InnerTokens.Count - 1);
		}

		public void AddCurrentKeySequenceAsPlainText()
		{
			if (!(LastToken.Type is PlainText && _lastToken != _mainToken))
				AddChildToken(new PlainText());
			LastToken.PlainText.Append(CurrentKeySequence);
		}

		public void AddChildToken(IToken tokenType)
		{
			var newToken = new TokenInfo(CurrentIndex, tokenType);
			LastToken.InnerTokens.Add(newToken);
			_lastToken = new TokenContainer(newToken, _lastToken);
		}

		public void CloseLastToken()
		{
			if (_lastToken == _mainToken) return;
			_lastToken.TokenInfo.TryClose(this);
			_lastToken = _lastToken.ParentToken;
		}
	}
}