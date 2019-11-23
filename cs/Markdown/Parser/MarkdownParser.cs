using System;
using System.Collections.Generic;
using System.Linq;
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
            Parsers = new Dictionary<TokenType, Action<Token, Stack<Element>>>
            {
                [TokenType.Text] = ParseTextToken,
                [TokenType.OpeningTag] = ParseOpeningTag,
                [TokenType.ClosingTag] = ParseClosingTag
            };
        }

        internal static Element Parse(IEnumerable<Token> tokens)
        {
            var root = new Element(null);
            var stack = new Stack<Element>();
            stack.Push(root);
            foreach (var token in tokens)
                Parsers[token.Type](token, stack);
            TransformSubtreeToText(root, TakeOutSubElements(stack, root));
            return root;
        }

        private static void ParseTextToken(Token token, Stack<Element> elements)
        {
            elements.Peek().ChildNodes.Add(new Text(token.Value));
        }

        private static void ParseOpeningTag(Token token, Stack<Element> elements)
        {
            var element = Elements[token.Lexeme](token.Value);
            if (elements.Peek().CanContain(element))
                elements.Push(element);
            else
                ParseTextToken(token, elements);
        }

        private static void ParseClosingTag(Token token, Stack<Element> elements)
        {
            var element = Elements[token.Lexeme](token.Value);
            if (elements.Any(opening => opening.GetType() == element.GetType()))
            {
                var subElements = TakeOutSubElements(elements, element);
                element = elements.Pop();
                TransformSubtreeToText(element, subElements);
                elements.Peek().ChildNodes.Add(element);
            }
            else
                ParseTextToken(token, elements);
        }

        private static void TransformSubtreeToText(Element parent, IEnumerable<INode> nodes)
        {
            foreach (var node in nodes)
            {
                var text = node.Type == NodeType.Text ? node : new Text(node.Value);
                parent.ChildNodes.Add(text);
                if (node.Type == NodeType.Element)
                    TransformSubtreeToText(parent, node.ChildNodes);
            }
        }

        private static IEnumerable<Element> TakeOutSubElements(Stack<Element> elements, Element parent)
        {
            var result = new Stack<Element>();
            while (elements.Peek().GetType() != parent.GetType())
                result.Push(elements.Pop());
            return result;
        }
    }
}