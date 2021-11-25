using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    internal class ParagraphParser
    {
        private List<Token> tokens;
        private Stack<Token> openedTokens;
        private Token upperToken;
        private StringBuilder current;
        private string paragraph;
        private static readonly List<char> specSymbols = 
            new List<char> {'\\', '_'};

        private void Initialize()
        {
            tokens = new List<Token>();
            openedTokens = new Stack<Token>();
            current = new StringBuilder();
        }

        public List<Token> TokenizeParagraph(string paragraph)
        {
            Initialize();
            this.paragraph = paragraph;
            for (var i = 0; i < paragraph.Length; i++)
            {
                var symbol = paragraph[i];
                switch (symbol)
                {
                    case '\\':
                        i = TryEscapeSymbol(i);
                        break;
                    case '_':
                        i = AddTagToken(i, symbol);
                        break;
                    case ' ':
                        current.Append(symbol);
                        AddTextToTokens();
                        current = new StringBuilder();
                        break;
                    default:
                        current.Append(symbol);
                        break;
                }
            }
            AddTextToTokens();
            return tokens;
        }

        private int AddTagToken(int i, char symbol)
        {
            Token token;
            var canClose = CanClose(i);
            if (i < paragraph.Length - 1 && paragraph[i + 1] == '_')
            {
                i++;
                if (!CanUseTag(CanOpen(i), canClose, typeof(ItalicText)))
                {
                    current.Append(symbol);
                    current.Append(paragraph[++i]);
                    return i;
                }
                token = new StrongText();
            }
            else
            {
                var withDigits = i < paragraph.Length - 1 && char.IsDigit(paragraph[i + 1]) ||
                                 i > 0 && char.IsDigit(paragraph[i - 1]);
                if (!CanUseTag(CanOpen(i), canClose, typeof(StrongText)) || withDigits)
                {
                    current.Append(symbol);
                    return i;
                }
                token = new ItalicText();
            }
            TryCloseTag(token, i);
            current = new StringBuilder();
            return i;
        }

        private bool CanUseTag(bool canOpen, bool canClose, Type tokenType)
            => canClose || canOpen &&
                openedTokens.Count == 0 || openedTokens.Peek().GetType() == tokenType;

        private void TryCloseTag(Token token, int i)
        {
            if (CanOpen(i) && CanClose(i))
                token.InText = true;
            if (openedTokens.Count == 0) 
            {
                AddTextToTokens();
                upperToken = token;
                tokens.Add(upperToken);
                openedTokens.Push(token);
                return;
            }
            var opened = openedTokens.Pop();
            var isClosed = opened.GetType() == token.GetType();
            AddTextToTokens(opened);

            if (isClosed)
                CloseToken(token, opened);
            else if (openedTokens.Count == 0)
                AddInnerTokenToOpened(token, opened);
            else
                CloseInvalidTag();
        }

        private void CloseToken(Token token, Token opened)
        {
            if (opened.InText && token.InText &&
                !(opened.HaveInner && opened.InnerTokens.Count == 1))
            {
                CloseInvalidTag();
                return;
            }
            if (openedTokens.Count == 0)
                upperToken = null;
            opened.Closed = true;
        }

        private void AddInnerTokenToOpened(Token token, Token opened)
        {
            openedTokens.Push(opened);
            openedTokens.Push(token);
            opened.InnerTokens.Add(token);
        }

        private bool CanClose(int i, int depth = 0)
            => --i > 1 && GetOpenNextStep(i, depth);

        private bool CanOpen(int i, int depth = 0)
            => ++i < paragraph.Length - 1 && GetOpenNextStep(i, depth);

        // Сам не понял почему это работает, но если в CanClose вызывать
        // CanClose то ломается пара тестов
        private bool GetOpenNextStep(int i, int depth = 0)
            => IsSpecSymbol(i) ?
                depth < 16 && CanOpen(i, depth + 1) :
                paragraph[i] != ' ';

        private bool IsSpecSymbol(int i)
            => specSymbols.Contains(paragraph[i]);

        private void CloseInvalidTag()
        {
            upperToken.Valid = false;
            upperToken.Closed = true;
            upperToken = null;
            openedTokens = new Stack<Token>();
        }

        private int TryEscapeSymbol(int i)
        {
            if (i < paragraph.Length - 2 && IsSpecSymbol(i + 1))
                i++;
            current.Append(paragraph[i]);
            return i;
        }

        private void AddTextToTokens
            (Token openedToken = null)
        {
            if (current.Length == 0)
                return;
            var value = current.ToString();
            openedToken ??= openedTokens.Count > 0 ? 
                openedTokens.Peek() : null;
            if (openedToken != null)
                openedToken.InnerTokens.Add(new Token(value));
            else
                tokens.Add(new Token(value));
        }
    }
}
