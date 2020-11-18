using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    class TokenReader
    {
        private readonly Stack<Token> openedTokens = new Stack<Token>();
        private Token currentToken;
        private readonly Dictionary<char, Action> symbolHandler;
        private readonly StringBuilder line;
        private bool containsOnlyDigits = true;
        private int position;


        public TokenReader(string inputLine)
        {
            line = new StringBuilder(inputLine);
            symbolHandler = new Dictionary<char, Action>();
            symbolHandler['_'] = HandleUnderline;
            symbolHandler['#'] = HandleHashSign;
            symbolHandler['\n'] = HandleEndOfLine;
        }

        public string Read(Token token)
        {
            position = 0;
            currentToken = token;
            while (position < line.Length)
            {
                HandleSymbol();
            }
            HandleEndOfLine();
            return line.ToString();
        }

        public void HandleSymbol()
        {
            CheckEscapeChar();
            if (symbolHandler.ContainsKey(line[position]))
                symbolHandler[line[position]]();
            else if (!char.IsDigit(line[position]))
                containsOnlyDigits = false;
            CheckTokenContainsTwoPartsOfDifferentWords();
            position++;
        }

        // Методы, отвечающие за обработку спец. символов

        private void HandleUnderline()
        {
            if (position != line.Length - 1 && line[position + 1] == '_')
            {
                if (HandleTripleUnderline())
                    return;
                HandleStrongTag();
            }
            else
                HandleItalicTag();
        }

        private void HandleHashSign()
        {
            if (position != line.Length - 1 && line[position + 1] == ' ')
            {
                var openedToken = new HeaderToken(position, 0, currentToken);
                OpenToken(openedToken);
            }
        }

        private void HandleEndOfLine()
        {
            position = position >= line.Length ? line.Length - 1 : position;
            while (openedTokens.Any())
            {
                if (openedTokens.Peek().Type == TokenType.Header)
                {
                    CloseToken(position);
                    openedTokens.Clear();
                }
                else
                    openedTokens.Pop();
            }
        }

        // Методы, отвечающие за обработку тегов

        private void HandleItalicTag()
        {
            if (openedTokens.Any() && openedTokens.Peek().Type == TokenType.Italic && !CheckSpaceBeforeTag())
            {
                TryCloseItalicToken();
            }
            else if (position == line.Length - 1 || line[position + 1] != ' ')
            {
                OpenItalicToken();
            }
        }

        private void HandleStrongTag()
        {
            if (openedTokens.Any() && !CheckSpaceBeforeTag() 
                                   && (openedTokens.Peek().Type == TokenType.Italic || openedTokens.Peek().Type == TokenType.Strong))
            {
                TryCloseStrongToken();
            }
            else if(!CheckSpaceAfterTag(new StrongToken().OpeningTag.Length))
            {
                OpenStrongToken();
            }
            position++;
        }

        private bool HandleTripleUnderline()
        {
            if (openedTokens.Any() && openedTokens.Peek().Type == TokenType.Italic
                                   && position < line.Length - 2 && line[position + 2] == '_')
            {
                if (CheckSpaceBeforeTag())
                    return true;
                CloseToken(position);
                return true;
            }
            return false;
        }

        // Методы, отвечающие за открытие и закрытие отдельных токенов

        private void TryCloseStrongToken()
        {
            if (openedTokens.Peek().Type == TokenType.Italic && openedTokens.Peek().Parent.Type == TokenType.Strong)
                ClosePreviousOpenedToken();
            else if (openedTokens.Peek().Type == TokenType.Strong && currentToken.Parent.Type != TokenType.Italic
                                                                  && !containsOnlyDigits)
                CloseToken(position);
        }

        private void OpenStrongToken()
        {
            var openedToken = new StrongToken(position, 0, currentToken);
            containsOnlyDigits = true;
            OpenToken(openedToken);
        }

        private void TryCloseItalicToken()
        {
            if (!CheckIntersectionOfTokens() && !containsOnlyDigits)
                CloseToken(position);
        }

        private void OpenItalicToken()
        {
            containsOnlyDigits = true;
            var openedToken = new ItalicToken(position, 0, currentToken);
            OpenToken(openedToken);
        }

        // Методы, для работы с токенами

        private void OpenToken(Token token)
        {
            openedTokens.Push(token);
            token.Parent = currentToken;
            currentToken = token;
        }

        private void CloseToken(int startOfClosingTagPosition)
        {
            currentToken = currentToken.Parent;
            var closedToken = openedTokens.Pop();
            closedToken.IsClosed = true;
            closedToken.Length = startOfClosingTagPosition - closedToken.StartPosition + closedToken.ClosingTag.Length;
            if (closedToken.Length != closedToken.ClosingTag.Length + closedToken.OpeningTag.Length)
                currentToken.SubTokens.Add(closedToken);
        }

        private void RemoveToken(Token token)
        {
            var parent = token.Parent;
            foreach (var closedSubToken in token.SubTokens.Where(token => token.IsClosed))
            {
                parent.SubTokens.Add(closedSubToken);
                closedSubToken.Parent = parent;
            }
            parent.SubTokens.Remove(token);
        }

        private void ClosePreviousOpenedToken()
        {
            var italicToken = openedTokens.Pop();
            CloseToken(position);
            openedTokens.Push(italicToken);
        }

        // Вспомогательные методы

        private bool CheckSpaceAfterTag(int tagLength)
        {
            return position != line.Length - tagLength && line[position + tagLength] == ' ';
        }

        private bool CheckSpaceBeforeTag()
        {
            return line[position - 1] == ' ';
        }

        private void CheckEscapeChar()
        {
            if (line[position] == '\\' && position != line.Length - 1
                                       && (symbolHandler.ContainsKey(line[position + 1]) || line[position + 1] == '\\'))
            {
                line.Remove(position, 1);
                position++;
            }
        }

        private void CheckTokenContainsTwoPartsOfDifferentWords()
        {
            if (line[position] == ' '
                && openedTokens.Any()
                && (openedTokens.Peek().Type == TokenType.Italic || openedTokens.Peek().Type == TokenType.Strong)
                && openedTokens.Peek().StartPosition != 0
                && line[openedTokens.Peek().StartPosition - 1] != ' '
                && !symbolHandler.ContainsKey(line[openedTokens.Peek().StartPosition - 1]))
                currentToken = openedTokens.Pop().Parent;
        }

        private bool CheckIntersectionOfTokens()
        {
            if (openedTokens.Peek().Parent.IsClosed)
            {
                RemoveToken(openedTokens.Peek().Parent);
                openedTokens.Pop();
                return true;
            }

            return false;
        }
    }
}
