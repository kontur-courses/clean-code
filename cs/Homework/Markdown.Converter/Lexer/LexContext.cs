using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Lexer.ConcreteLexers;
using Markdown.Tokens;

namespace Markdown.Lexer
{
    public class LexContext
    {
        public const char EndOfFile = '\0';
        private readonly HashSet<char> specSymbols;
        private readonly string text;
        private readonly Dictionary<char, Func<Token>> tokenSymbolsHandlers;

        public int Position;

        public LexContext(string text)
        {
            this.text = text;
            tokenSymbolsHandlers = new Dictionary<char, Func<Token>>
            {
                {'_', () => new UnderscoreLexer(this).Lex()},
                {'\\', () => Token.Escape},
                {'\n', () => Token.NewLine},
                {'#', () => new Header1Lexer(this).Lex()},
                {'!', () => new OpenImageAltLexer(this).Lex()},
                {']', () => new CloseImageAltLexer(this).Lex()},
                {')', () => Token.CloseImageTag}
            };
            specSymbols = tokenSymbolsHandlers.Keys.ToHashSet();
        }

        public char Current => Peek(0);

        public char Lookahead => Peek(1);

        public IEnumerable<Token> Lex()
        {
            do
            {
                yield return tokenSymbolsHandlers.GetValueOrDefault(Current,
                    () => new TextLexer(specSymbols, this).Lex())();
                Position++;
            } while (Current != EndOfFile);
        }

        public void NextSymbol() => Position++;

        private char Peek(int offset)
        {
            var index = Position + offset;

            return index >= text.Length ? EndOfFile : text[index];
        }
    }
}