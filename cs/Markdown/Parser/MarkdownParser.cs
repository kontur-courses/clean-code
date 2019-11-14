using System;
using System.Collections.Generic;
using Markdown.Lexer;

namespace Markdown.Parser
{
    internal class MarkdownParser
    {
        internal static readonly IDictionary<string, Func<INode, string, Element>> Elements;

        static MarkdownParser()
        {
            Elements = new Dictionary<string, Func<INode, string, Element>>()
            {
                ["_"] = (parent, value) => new MarkdownItalicElement(parent, value),
                ["__"] = (parent, value) => new MarkdownBoldElement(parent, value)
            };
        }

        internal static Element Parse(ISequence<Token> input)
        {
            throw new NotImplementedException();
        }
    }
}