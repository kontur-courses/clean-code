using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    class TokenReader
    {
        public Stack<IToken> OpenedTokens { get; }
        private readonly SymbolHandler symbolHandler;
        private readonly Dictionary<TokenType, Type> tokenTypes;
        public StringBuilder Line { get; }
        public int Position { get; private set; }


        public TokenReader(string inputLine)
        {
            OpenedTokens = new Stack<IToken>();
            Line = new StringBuilder(inputLine);
            symbolHandler = new SymbolHandler(this);
            tokenTypes = new Dictionary<TokenType, Type>();
            tokenTypes[TokenType.Strong] = typeof(StrongToken);
            tokenTypes[TokenType.Italic] = typeof(ItalicToken);
            tokenTypes[TokenType.Header] = typeof(HeaderToken);
            tokenTypes[TokenType.Reference] = typeof(ReferenceToken);
        }

        public string Read(Token token)
        {
            Position = 0;
            OpenedTokens.Push(token);
            while (Position < Line.Length)
            {
                HandleSymbol();
            }
            symbolHandler.HandleEndOfLine();
            return Line.ToString();
        }

        public void HandleSymbol()
        {
            CheckEscapeChar();
            if (symbolHandler.CanHandle(Line[Position]))
                symbolHandler.Handle(Line[Position]);
            else if (OpenedTokens.Peek().ContainsOnlyDigits && !char.IsDigit(Line[Position]))
                foreach (var token in OpenedTokens)
                    token.ContainsOnlyDigits = false;
            CheckTokenContainsTwoPartsOfDifferentWords();
            Position++;
        }

        public void HandleTag(IToken token)
        {
            if (OpenedTokens.Any() && !CheckSpaceBeforeTag() && !CheckIntersectionOfTokens())
            {
                if (TryCloseFirstAvailableToken(token))
                    return;
            }
            if(!CheckSpaceAfterTag(token.OpeningTag.Length))
            {
                OpenToken(token);
            }
            Position += token.OpeningTag.Length - 1;
        }

        private bool TryCloseFirstAvailableToken(IToken token)
        {
            var openedToken = OpenedTokens.Peek();
            while (openedToken.Type != TokenType.Simple)
            {
                if (openedToken.Type == token.Type)
                {
                    openedToken.CloseToken(OpenedTokens, Position);
                    Position += token.ClosingTag.Length - 1;
                    return true;
                }
                openedToken = openedToken.Parent;
            }

            return false;
        }

        private void OpenToken(IToken token)
        {
            if (token.NestingLevel > OpenedTokens.Peek().NestingLevel)
                IToken.Open((Token)token, OpenedTokens);
        }

        // Вспомогательные методы

        public bool CheckSpaceAfterTag(int tagLength)
        {
            return Position != Line.Length - tagLength && Line[Position + tagLength] == ' ';
        }

        public bool CheckSpaceBeforeTag()
        {
            return Position != 0 && Line[Position - 1] == ' ';
        }

        private void CheckEscapeChar()
        {
            if (Line[Position] == '\\' && Position != Line.Length - 1
                                       && (symbolHandler.CanHandle(Line[Position + 1]) || Line[Position + 1] == '\\'))
            {
                Line.Remove(Position, 1);
                OpenedTokens.Peek().Length--;
                Position++;
            }
        }

        private void CheckTokenContainsTwoPartsOfDifferentWords()
        {
            if (Line[Position] == ' '
                && OpenedTokens.Any()
                && OpenedTokens.Peek().StartPosition != 0
                && Line[OpenedTokens.Peek().StartPosition - 1] != ' '
                && !symbolHandler.CanHandle(Line[OpenedTokens.Peek().StartPosition - 1]))
                OpenedTokens.Pop();
        }

        private bool CheckIntersectionOfTokens()
        {
            if (OpenedTokens.Peek().Type != TokenType.Simple && OpenedTokens.Peek().Parent.IsClosed)
            {
                IToken.Remove(OpenedTokens.Peek().Parent);
                OpenedTokens.Pop();
                return true;
            }

            return false;
        }
    }
}
