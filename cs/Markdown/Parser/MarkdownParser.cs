using System;
using System.Collections.Generic;
using Markdown.Lexer;

namespace Markdown.Parser
{
    internal static class MarkdownParser
    {
        private static readonly IDictionary<Lexeme, Func<string, Element>> Elements;
        private static readonly IDictionary<TokenType, Action<Token, Stack<Element>>> Parsers;

        static MarkdownParser()
        {
            Elements = new Dictionary<Lexeme, Func<string, Element>>
            {
                [LexemeDefinitions.Italic] = (value) => new MarkdownItalicElement(value),
                [LexemeDefinitions.Bold] = (value) => new MarkdownBoldElement(value)
            };
        }

        internal static Element Parse(IEnumerable<Token> tokens)
        {
            throw new NotImplementedException();
        }
    }
}