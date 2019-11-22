using System;
using System.Collections.Generic;
using Markdown.Lexer;

namespace Markdown.Parser
{
    internal static class MarkdownParser
    {
        internal static readonly IDictionary<Lexeme, Func<INode, string, Element>> Elements;

        static MarkdownParser()
        {
            Elements = new Dictionary<Lexeme, Func<INode, string, Element>>()
            {
                [LexemeDefinitions.Italic] = (parent, value) => new MarkdownItalicElement(parent, value),
                [LexemeDefinitions.Bold] = (parent, value) => new MarkdownBoldElement(parent, value)
            };
        }

        internal static Element Parse(IEnumerable<Token> tokens)
        {
            throw new NotImplementedException();
        }
    }
}