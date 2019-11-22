using System.Collections.Generic;
using System.Text;
using Markdown.Features;

namespace Markdown
{
	internal class TokenizerContextState
	{
		public TokenStack TokenStack { get; }
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
		public TokenInfo MainToken => TokenStack.MainToken.TokenInfo;
		public TokenInfo LastToken => TokenStack.LastToken.TokenInfo;
		public StringBuilder CurrentKeySequence { get; } = new StringBuilder();
		public bool Shielded { get; set; }
		public string SourceText { get; }
		public int CurrentIndex { get; private set; }
		public char CurrentChar { get; private set; }
		public TokenStack.TokenContainer TokenToClose { get; set; }
		public IToken TokenToOpen { get; set; }

		public TokenizerContextState(string sourceText)
		{
			SourceText = sourceText;
			TokenStack = new TokenStack(new TokenInfo(0, new PlainText()));
		}

		public void HandleChar(int currentIndex)
		{
			CurrentIndex = currentIndex;
			CurrentChar = SourceText[currentIndex];
			Shielded = Shielded || CurrentChar == '\\';
			if (CurrentChar == PlainTextReadingClosingChar && !Shielded) ReadingAsPlainText = false;
			CurrentKeySequence.Append(CurrentChar);
			ReadingKeySequence = _readingKeySequence && !Shielded;
		}

		public void TryCloseToken()
		{
			var currentToken = TokenStack.LastToken;
			while (currentToken != null && currentToken != TokenStack.MainToken)
			{
				if (!(currentToken.TokenInfo.Type is PlainText) &&
				    currentToken.TokenInfo.CanBeClosed(this))
				{
					TokenToClose = currentToken;
					return;
				}
				currentToken = currentToken.Parent;
			}
		}

		public void CloseTokens()
		{
			RemoveRedundantToken();
			var tokenType = TokenToClose.TokenInfo.Type;
			var indexOffset = tokenType is IComplexToken || tokenType is IComplexTokenBlock ? 0 : 1;
			TokenToClose.TokenInfo.Close(CurrentIndex - indexOffset);
			TokenStack.Remove(TokenToClose);
			ReadingAsPlainText = false;
			PlainTextReadingClosingChar = new char();
			TokenToClose = null;
		}

		private void RemoveRedundantToken()
		{
			if (LastToken.InnerTokens.Count != 0 || LastToken.Closed || LastToken.Type is PlainText ||
			    LastToken.Type is IComplexTokenBlock) return;
			var extraTokenParent = TokenStack.LastToken.Parent.TokenInfo;
			extraTokenParent.InnerTokens.RemoveAt(extraTokenParent.InnerTokens.Count - 1);
		}

		public void AddCurrentKeySequenceAsPlainText()
		{
			if ((!(LastToken.Type is PlainText) || LastToken == MainToken) && 
			    !ReadingAsPlainText && CurrentKeySequence.Length > 0)
				AddChildToken(new PlainText());
			LastToken.PlainText.Append(CurrentKeySequence);
		}

		private void AddChildToken(IToken tokenType)
		{
			var newToken = new TokenInfo(CurrentIndex - CurrentKeySequence.Length, tokenType);
			LastToken.InnerTokens.Add(newToken);
			TokenStack.Push(newToken);
		}

		public void CloseLastPlainText()
		{
			if (LastToken == MainToken) return;
			LastToken.Close(CurrentIndex - 1);
			TokenStack.Remove(TokenStack.LastToken);
		}

		public void TryOpenToken(IToken tokenType)
		{
			if (!tokenType.IsOpeningKeySequence(this)) return;
			TokenToOpen = tokenType;
		}
		
		public void OpenToken()
		{
			if (!(TokenToOpen is IComplexTokenBlock))
			{
				AddChildToken(TokenToOpen);
				ReadingAsPlainText = TokenToOpen.PlainTextContent;
				if (ReadingAsPlainText)
					PlainTextReadingClosingChar = TokenToOpen.ClosingSequence[0];
			}

			if (!(TokenToOpen is IComplexTokenBlock childToken) &&
			    (!(TokenToOpen is IComplexToken complexToken) ||
			     !complexToken.IsOpeningSequenceForChild(this, out childToken))) return;
			AddComplexTokenBlock(childToken);
			ReadingKeySequence = false;
		}

		private void AddComplexTokenBlock(IComplexTokenBlock childToken)
		{
			var childTokenInfo = new TokenInfo(CurrentIndex, childToken);
			LastToken.Blocks.Add(childTokenInfo);
			TokenStack.Push(childTokenInfo);
			ReadingAsPlainText = childToken.PlainTextContent;
			if (ReadingAsPlainText)
				PlainTextReadingClosingChar = childToken.ClosingSequence[0];
		}
	}
}