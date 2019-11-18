using System.Collections.Generic;
using System.Text;
using Markdown.Features;

namespace Markdown
{
	internal class TokenizerContextState
	{
		private readonly Dictionary<string, IToken> _supportedKeySequences;
		private readonly HashSet<char> _supportedKeyChars;
		private readonly TokenStack _tokenStack;
		
		public char PlainTextReadingClosingChar;
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
		private bool _readingAsPlainText;
		public bool ReadingAsPlainText
		{
			get => _readingAsPlainText;
			set
			{
				if (value) ReadingKeySequence = false;
				_readingAsPlainText = value;
			}
		}
		public TokenInfo MainToken => _tokenStack.MainToken.TokenInfo;
		public TokenInfo LastToken => _tokenStack.LastToken.TokenInfo;
		public StringBuilder CurrentKeySequence { get; } = new StringBuilder();
		public bool Shielded { get; set; }
		public string SourceText { get; }
		public int CurrentIndex { get; private set; }
		public char CurrentChar { get; private set; }

		public TokenizerContextState(string sourceText, HashSet<char> supportedKeyChars, 
									Dictionary<string, IToken> supportedKeySequences)
		{
			SourceText = sourceText;
			_tokenStack = new TokenStack(new TokenInfo(0, new PlainText()));
			_supportedKeyChars = supportedKeyChars;
			_supportedKeySequences = supportedKeySequences;
		}

		public void Update(int currentIndex)
		{
			UpdateCurrentSequence(currentIndex);
			UpdateReadingKeySequence();
			if (!ReadingKeySequence) return;
			if (LastToken.Type is PlainText) CloseLastToken();
			var isKeySequence = CheckIsKeySequence(out var tokenType);
			if (isKeySequence)
			{
				if (TryCloseToken())
					return;
				if (tokenType.IsOpeningKeySequence(this))
				{
					OpenToken(tokenType);
					return;
				}
			}
			if (_supportedKeyChars.Contains(CurrentChar) && currentIndex < SourceText.Length - 1)
				return;
			if (LastToken.InnerTokens.Count == 0 && !(LastToken.Type is IComplexToken))
				CurrentKeySequence.Remove(0, LastToken.Type.OpeningSequence.Length);
			AddCurrentKeySequenceAsPlainText();
			ReadingKeySequence = false;
		}

		private void UpdateReadingKeySequence()
		{
			if (ReadingKeySequence) return;
			var isKeyChar = _supportedKeyChars.Contains(CurrentChar) &&
			                !ReadingAsPlainText;
			if (isKeyChar && Shielded)
			{
				isKeyChar = false;
				RemoveLastPlainTextChar();
				Shielded = false;
			}

			ReadingKeySequence = isKeyChar;
			if (isKeyChar) return;
			AddCurrentKeySequenceAsPlainText();
			CurrentKeySequence.Clear();
		}

		private void UpdateCurrentSequence(int currentIndex)
		{
			CurrentIndex = currentIndex;
			CurrentChar = SourceText[currentIndex];
			Shielded = Shielded || CurrentChar == '\\';
			if (CurrentChar == PlainTextReadingClosingChar && !Shielded) ReadingAsPlainText = false;
			CurrentKeySequence.Append(CurrentChar);
			ReadingKeySequence = _readingKeySequence && !Shielded;
		}

		private void RemoveLastPlainTextChar()
		{
			var lastCharIndex = LastToken.PlainText.Length - 1;
			LastToken.PlainText.Remove(lastCharIndex, 1);
		}

		private bool CheckIsKeySequence(out IToken tokenType)
		{
			if (_supportedKeySequences.TryGetValue(CurrentKeySequence.ToString(), out tokenType))
				return true;
			tokenType = new PlainText();
			return false;
		}

		private bool TryCloseToken()
		{
			var currentToken = _tokenStack.LastToken;
			while (currentToken != null && currentToken != _tokenStack.MainToken)
			{
				if (!(currentToken.TokenInfo.Type is PlainText) && 
				    currentToken.TokenInfo.TryClose(this))
				{
					RemoveRedundantToken();
					_tokenStack.Remove(currentToken);
					ReadingKeySequence = false;
					ReadingAsPlainText = false;
					PlainTextReadingClosingChar = new char();
					return true;
				}
				currentToken = currentToken.Parent;
			}
			return false;
		}

		private void RemoveRedundantToken()
		{
			if (LastToken.InnerTokens.Count != 0 || LastToken.Closed || LastToken.Type is PlainText ||
			    LastToken.Type is IComplexTokenBlock) return;
			var extraTokenParent = _tokenStack.LastToken.Parent.TokenInfo;
			extraTokenParent.InnerTokens.RemoveAt(extraTokenParent.InnerTokens.Count - 1);
		}

		private void AddCurrentKeySequenceAsPlainText()
		{
			if ((!(LastToken.Type is PlainText) || LastToken == MainToken) && !ReadingAsPlainText)
				AddChildToken(new PlainText());
			LastToken.PlainText.Append(CurrentKeySequence);
		}

		private void AddChildToken(IToken tokenType)
		{
			var newToken = new TokenInfo(CurrentIndex, tokenType);
			LastToken.InnerTokens.Add(newToken);
			_tokenStack.Push(newToken);
		}

		private void CloseLastToken()
		{
			if (LastToken == MainToken) return;
			LastToken.TryClose(this);
			_tokenStack.Remove(_tokenStack.LastToken);
		}

		private void OpenToken(IToken tokenType)
		{
			if (!(tokenType is IComplexTokenBlock))
			{
				if (LastToken.Type is PlainText || LastToken.InnerTokens.Count > 0 || 
				    LastToken.Type is IComplexTokenBlock)
					AddChildToken(tokenType);
				else
					LastToken.Type = tokenType;
				ReadingAsPlainText = tokenType.PlainTextContent;
				if (ReadingAsPlainText)
					PlainTextReadingClosingChar = tokenType.ClosingSequence[0];
			}

			if (tokenType is IComplexTokenBlock childToken ||
			    tokenType is IComplexToken complexToken &&
			    complexToken.IsOpeningSequenceForChild(this, out childToken))
			{
				var childTokenInfo = new TokenInfo(CurrentIndex, childToken);
				LastToken.Blocks.Add(childTokenInfo);
				_tokenStack.Push(childTokenInfo);
				ReadingAsPlainText = childToken.PlainTextContent;
				if (ReadingAsPlainText)
					PlainTextReadingClosingChar = childToken.ClosingSequence[0];
			}
			ReadingKeySequence = false;
		}
	}
}