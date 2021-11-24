using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Markdown
{
    public class ParagraphParser
    {
        private List<Token> tokens;
        private Stack<Token> openedTokens;
        private Token upperToken;
        private StringBuilder current;
        private string paragraph;

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
            var canOpen = false;
            var canClose = i <= 1 || paragraph[i - 1] != ' ';
            if (i < paragraph.Length - 1 && paragraph[i + 1] == '_')
            {
                i++;
                canOpen = i >= paragraph.Length - 1 || paragraph[i + 1] != ' ';
                if (!CanUseTag(canOpen, canClose, typeof(ItalicText)))
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
                canOpen = i >= paragraph.Length - 1 || paragraph[i + 1] != ' ';
                if (!CanUseTag(canOpen, canClose, typeof(StrongText)) || withDigits)
                {
                    current.Append(symbol);
                    return i;
                }
                token = new ItalicText();
            }
            TryCloseTag(token);
            current = new StringBuilder();
            return i;
        }

        private bool CanUseTag(bool canOpen, bool canClose, Type tokenType)
            => canClose || canOpen &&
                openedTokens.Count == 0 || openedTokens.Peek().GetType() == tokenType;

        private void TryCloseTag(Token token)
        {
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
            {
                if (openedTokens.Count == 0)
                    upperToken = null;
                opened.Closed = true;
            }
            else if (openedTokens.Count == 0)
            {
                openedTokens.Push(opened);
                openedTokens.Push(token);
                opened.InnerTokens.Add(token);
            }
            else
            {
                upperToken.Valid = false;
                upperToken.Closed = true;
                upperToken = null;
                openedTokens = new Stack<Token>();
            }
        }

        private int TryEscapeSymbol(int i)
        {
            if (i < paragraph.Length - 2 && 
                (paragraph[i + 1] == '_' || paragraph[i + 1] == '\\'))
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
