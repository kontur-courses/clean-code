using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    class SymbolHandler
    {
        private readonly Dictionary<char, Action> symbolHandler;
        private readonly TokenReader reader;
        private int position;
        private StringBuilder line;

        public SymbolHandler(TokenReader reader)
        {
            this.reader = reader;
            line = new StringBuilder();
            symbolHandler = new Dictionary<char, Action>();
            symbolHandler['_'] = HandleUnderline;
            symbolHandler['#'] = HandleHashSign;
            symbolHandler['\n'] = HandleEndOfLine;
            symbolHandler['$'] = HandleReference;
        }

        public bool CanHandle(char symbol) => symbolHandler.ContainsKey(symbol);

        public void Handle(char symbol)
        {
            line = reader.Line;
            position = reader.Position;
            if (symbolHandler.ContainsKey(symbol))
                symbolHandler[symbol]();
        }

        private void HandleUnderline()
        {
            IToken token;
            if (position != line.Length - 1 && line[position + 1] == '_')
            {
                if (HandleTripleUnderline())
                    return;
                token = new StrongToken(position, 0, reader.OpenedTokens.Peek());
            }
            else
                token = new ItalicToken(position, 0, reader.OpenedTokens.Peek());
            reader.HandleTag(token);
        }

        private void HandleHashSign()
        {
            if (reader.CheckSpaceAfterTag("#".Length))
            {
                reader.HandleTag(new HeaderToken(position, 0, reader.OpenedTokens.Peek()));
            }
        }

        public void HandleEndOfLine()
        {
            position = reader.Position;
            while (reader.OpenedTokens.Count != 1)
            {
                if (reader.OpenedTokens.Peek().Type == TokenType.Header)
                {
                    IToken.Close(reader.OpenedTokens, position);
                }
                else
                    reader.OpenedTokens.Pop();
            }
        }

        private bool HandleTripleUnderline()
        {
            if (reader.OpenedTokens.Any() && reader.OpenedTokens.Peek().Type == TokenType.Italic
                                          && position < line.Length - 2 && line[position + 2] == '_')
            {
                if (reader.CheckSpaceBeforeTag())
                    return true;
                IToken.Close(reader.OpenedTokens, position);
                return true;
            }
            return false;
        }

        private void HandleReference()
        {
            var reference = new StringBuilder();
            var i = position + 1;
            while (i != line.Length && line[i] != '!')
            {
                reference.Append(line[i]);
                i++;
            }
            if (reference.Length == 0 && i != line.Length)
                return;
            var token = new ReferenceToken(position, 0, 
                    reference.ToString(), reader.OpenedTokens.Peek());
            reader.HandleTag(token);
        }
    }
}
