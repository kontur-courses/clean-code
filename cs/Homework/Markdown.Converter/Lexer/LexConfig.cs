using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Lexer.ConcreteLexers;
using Markdown.Tokens;

namespace Markdown.Lexer
{
    public static class LexConfig
    {
        private static readonly Dictionary<char, Func<LexContext, Token>> TokenSymbolsHandlers;
        private static readonly HashSet<char> SpecSymbols;
        private static readonly Func<LexContext, Token> TextLexer = ctx => new TextLexer(SpecSymbols, ctx).Lex();

        static LexConfig()
        {
            TokenSymbolsHandlers = new Dictionary<char, Func<LexContext, Token>>
            {
                {'_', ctx => new UnderscoreLexer(ctx).Lex()},
                {'\\', _ => Token.Escape},
                {'\n', _ => Token.NewLine},
                {'#', ctx => new Header1Lexer(ctx).Lex()},
                {'!', ctx => new OpenImageAltLexer(ctx).Lex()},
                {']', ctx => new CloseImageAltLexer(ctx).Lex()},
                {')', _ => Token.CloseImageTag}
            };

            SpecSymbols = TokenSymbolsHandlers.Keys.ToHashSet();
        }

        public static Func<LexContext, Token> GetLexer(char symbol)
            => TokenSymbolsHandlers.GetValueOrDefault(symbol, TextLexer);
    }
}