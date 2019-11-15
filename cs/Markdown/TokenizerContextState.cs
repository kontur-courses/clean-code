using System;
using System.Text;
using Markdown.Features;

namespace Markdown
{
	internal class TokenizerContextState
	{
		private TokenContainer _mainToken;
		private TokenContainer _lastToken;

		public TokenInfo MainToken => _mainToken.TokenInfo;
		public TokenInfo LastToken => _lastToken.TokenInfo;
		public StringBuilder CurrentKeySequence { get; } = new StringBuilder();
		public bool ReadingKeySequence { get; set; }
		public bool Shielded { get; private set; }
		public string SourceText { get; }
		public int CurrentIndex { get; private set; }
		public string CurrentChar { get; private set; }

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
			CurrentChar = SourceText[currentIndex].ToString();
			Shielded = !Shielded && CurrentChar == "\\";
			if (Shielded)
				CurrentKeySequence.Clear();
		}

		public bool TryCloseToken()
		{
			var currentToken = _lastToken;
			while (currentToken != null && currentToken != _mainToken)
			{
				if (currentToken.TokenInfo.TryClose(this))
				{
					_lastToken = currentToken.ParentToken;
					return true;
				}
				currentToken = currentToken.ParentToken;
			}
			return false;
		}

		public void AddCurrentCharAsPlainText()
		{
			if (!(LastToken.Type is PlainText))
			{
				var newToken = new TokenInfo(CurrentIndex, new PlainText());
				LastToken.InnerTokens.Add(newToken);
				_lastToken = new TokenContainer(newToken, _lastToken);
			}
			LastToken.PlainText.Append(CurrentChar);
		}

		public void AddChildToken(IToken tokenType)
		{
//			if (_lastToken != _mainToken && LastToken.Type is PlainText)
//			{
//				_lastToken.TokenInfo.TryClose(this);
//				_lastToken = _lastToken.ParentToken;
//			}
			var newToken = new TokenInfo(CurrentIndex, tokenType);
			LastToken.InnerTokens.Add(newToken);
			_lastToken = new TokenContainer(newToken, _lastToken);
		}
	}
}