using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using Markdown.MarkdownDocument;
using Markdown.MarkdownDocument.Inline;

namespace Markdown
{
    public static class Parser
    {
        private class Delimiter
        {
            public enum DelimiterType
            {
                Opener,
                Closer,
                Both
            }

            public bool IsOpener() => Type == DelimiterType.Opener || Type == DelimiterType.Both;
            public bool IsCloser() => Type == DelimiterType.Closer || Type == DelimiterType.Both;

            public DelimiterType Type;
            public LinkedListNode<IInline> FirstNode, LastNode;
            public int Count;
            public bool Active = true;
            public string Value;

            public Delimiter(string value, bool isPotentialOpener, bool isPotentialCloser, int count,
                LinkedListNode<IInline> firstNode, LinkedListNode<IInline> lastNode)
            {
                if (isPotentialCloser && isPotentialOpener)
                    Type = DelimiterType.Both;
                else if (isPotentialOpener)
                    Type = DelimiterType.Opener;
                else if (isPotentialCloser)
                    Type = DelimiterType.Closer;

                Count = count;

                FirstNode = firstNode;
                LastNode = lastNode;
            }
        }

        public static Line ParseLine(IEnumerable<IInline> elements)
        {
            var inlineElements = elements;
            inlineElements = ParseLinks(inlineElements);
            inlineElements = ParseEmphasisAndStrongEmphasis(inlineElements);
            inlineElements = RemoveStrongEmphasisInsideEmphasis(inlineElements);
            return new Line(inlineElements);
        }

        private static IEnumerable<IInline> ParseLinks(IEnumerable<IInline> elements)
        {
            var parsedElements = new LinkedList<IInline>(elements);
            var delimiters = GetLinkDelimiters(parsedElements);

            var bottomNode = delimiters.First;
            while (bottomNode != null)
            {
                var currentNode = bottomNode.Next;
                while (currentNode != null && 
                       (!currentNode.Value.IsCloser() || !currentNode.Value.Active)
                       && currentNode.Value.Value != ")")
                    currentNode = currentNode.Next;
                if (currentNode == null) break;

                bottomNode = currentNode;
                var closingNode = currentNode;
                var closingDelimiter = closingNode.Value;

                currentNode = currentNode.Previous;
                while (currentNode != null && !currentNode.Value.Active && currentNode.Value.Value != "](")
                    currentNode = currentNode.Previous;
                if (currentNode == null) continue;

                var interNode = currentNode;
                var interDelimiter = interNode.Value;
                
                currentNode = currentNode.Previous;
                while (currentNode != null && 
                       (!currentNode.Value.IsOpener() || !currentNode.Value.Active)
                       && currentNode.Value.Value != "[")
                    currentNode = currentNode.Previous;
                if (currentNode == null) continue;

                var openingNode = currentNode;
                var openingDelimiter = openingNode.Value;

                openingDelimiter.Active = false;
                interDelimiter.Active = false;
                closingDelimiter.Active = false;

                var text = parsedElements.GetAsList(openingDelimiter.LastNode.Next, interDelimiter.FirstNode);
                var address = parsedElements.GetAsList(interDelimiter.LastNode.Next, closingDelimiter.FirstNode);
                
                var link = new Link(text, address);

                parsedElements.AddBefore(openingDelimiter.FirstNode, link);
                parsedElements.RemoveRange(openingDelimiter.FirstNode, closingDelimiter.LastNode);
            }

            return parsedElements;
        }
        
        private static IEnumerable<IInline> RemoveStrongEmphasisInsideEmphasis(IEnumerable<IInline> inlineElements,
            bool isInsideEmphasis = false)
        {
            var parsedElements = new LinkedList<IInline>(inlineElements);

            var currentNode = parsedElements.First;
            while (currentNode != null)
            {
                var nextNode = currentNode.Next;
                if (currentNode.Value is StrongEmphasis strongEmphasis)
                {
                    var strongEmphasisContent = strongEmphasis.Content as IInline[] ?? strongEmphasis.Content.ToArray();
                    strongEmphasis.Content = RemoveStrongEmphasisInsideEmphasis(strongEmphasisContent, isInsideEmphasis);
                    if (isInsideEmphasis)
                    {
                        parsedElements.AddBefore(currentNode, Lexeme.CreateFromChar('_'));
                        parsedElements.AddBefore(currentNode, Lexeme.CreateFromChar('_'));
                        foreach (var element in strongEmphasisContent)
                        {
                            parsedElements.AddBefore(currentNode, element);
                        }

                        parsedElements.AddAfter(currentNode, Lexeme.CreateFromChar('_'));
                        parsedElements.AddAfter(currentNode, Lexeme.CreateFromChar('_'));
                        parsedElements.Remove(currentNode);
                    }
                }
                else if (currentNode.Value is Emphasis emphasis)
                {
                    emphasis.Content = RemoveStrongEmphasisInsideEmphasis(emphasis.Content, true);
                }
                else if (currentNode.Value is IInlineWithContent inlineWithContent)
                {
                    inlineWithContent.Content = RemoveStrongEmphasisInsideEmphasis(inlineWithContent.Content, isInsideEmphasis);
                }

                currentNode = nextNode;
            }

            return parsedElements;
        }

        private static IEnumerable<IInline> ParseEmphasisAndStrongEmphasis(IEnumerable<IInline> elements)
        {
            var parsedElements = new LinkedList<IInline>(elements);
            foreach (var element in parsedElements)
            {
                if (element is IInlineWithContent inlineWithContent)
                {
                    inlineWithContent.Content = ParseEmphasisAndStrongEmphasis(inlineWithContent.Content);
                }
            }
            
            var delimiters = GetDelimiters(parsedElements);

            var bottomNode = delimiters.First;
            while (bottomNode != null)
            {
                var currentNode = bottomNode.Next;
                while (currentNode != null && (!currentNode.Value.IsCloser() || !currentNode.Value.Active))
                    currentNode = currentNode.Next;
                if (currentNode == null) break;

                bottomNode = currentNode;
                var closingNode = currentNode;
                var closingDelimiter = closingNode.Value;
                var length = closingDelimiter.Count;

                currentNode = currentNode.Previous;
                while (currentNode != null && (!currentNode.Value.IsOpener() || !currentNode.Value.Active ||
                                               currentNode.Value.Count != length))
                    currentNode = currentNode.Previous;
                if (currentNode == null) continue;

                var openingNode = currentNode;
                var openingDelimiter = openingNode.Value;

                openingDelimiter.Active = false;
                closingDelimiter.Active = false;

                var inlineWithContent = length switch
                {
                    1 => (IInlineWithContent) new Emphasis(parsedElements.GetAsList(openingDelimiter.LastNode.Next,
                        closingDelimiter.FirstNode)),
                    2 => new StrongEmphasis(parsedElements.GetAsList(openingDelimiter.LastNode.Next,
                        closingDelimiter.FirstNode)),
                    _ => new StrongEmphasis(new List<IInline>()
                    {
                        new Emphasis(parsedElements.GetAsList(openingDelimiter.LastNode.Next,
                            closingDelimiter.FirstNode))
                    })
                };

                parsedElements.AddBefore(openingDelimiter.FirstNode, inlineWithContent);
                parsedElements.RemoveRange(openingDelimiter.FirstNode, closingDelimiter.LastNode);
            }

            return parsedElements;
        }

        private static LinkedList<Delimiter> GetDelimiters(LinkedList<IInline> elements)
        {
            var currentNode = elements.First;
            var delimiters = new LinkedList<Delimiter>();

            while (currentNode != null)
            {
                var currentElement = currentNode.Value;
                if (currentElement is Lexeme lexeme && !lexeme.IsEscaped() && lexeme.Value == "_")
                {
                    var length = 1;
                    bool isPotentialOpener = false, isPotentialCloser = false;
                    if (currentNode.Previous != null && currentNode.Previous is var previousNode)
                    {
                        if (previousNode.Value is Lexeme previousLexeme && !previousLexeme.IsWhitespace() ||
                            !(previousNode.Value is Lexeme))
                            isPotentialCloser = true;
                    }

                    var nextNode = currentNode.Next;
                    while (nextNode != null)
                    {
                        if (nextNode.Value is Lexeme nextLexeme && !nextLexeme.IsEscaped() && nextLexeme.Value == "_")
                        {
                            length += 1;
                            nextNode = nextNode.Next;
                        }
                        else
                            break;
                    }

                    if (nextNode == null)
                    {
                        nextNode = elements.Last;
                    }
                    else
                    {
                        if (nextNode.Value is Lexeme nextLexeme && !nextLexeme.IsWhitespace() ||
                            !(nextNode.Value is Lexeme))
                        {
                            isPotentialOpener = true;
                        }

                        nextNode = nextNode.Previous;
                    }

                    if (isPotentialOpener || isPotentialCloser)
                        delimiters.AddLast(new Delimiter("_", isPotentialOpener, isPotentialCloser, length, currentNode,
                            nextNode));

                    currentNode = nextNode?.Next;
                }
                else
                    currentNode = currentNode.Next;
            }

            return delimiters;
        }

        private static LinkedList<Delimiter> GetLinkDelimiters(LinkedList<IInline> elements)
        {
            var currentNode = elements.First;
            var delimiters = new LinkedList<Delimiter>();

            while (currentNode != null)
            {
                var currentElement = currentNode.Value;
                if (currentElement is Lexeme lexeme && !lexeme.IsEscaped())
                {
                    if (lexeme.Value == "[")
                    {
                        delimiters.AddLast(new Delimiter("[", true, false, 1,
                            currentNode, currentNode));
                    }

                    if (lexeme.Value == "]" && currentNode.Next?.Value is Lexeme nextLexeme &&
                        !nextLexeme.IsEscaped() && nextLexeme.Value == "(")
                    {
                        delimiters.AddLast(new Delimiter("](", false, false, 1,
                            currentNode, currentNode.Next));
                    }

                    if (lexeme.Value == ")")
                    {
                        delimiters.AddLast(new Delimiter(")", false, true, 1,
                            currentNode, currentNode));
                    }
                }

                currentNode = currentNode.Next;
            }

            return delimiters;
        }
    }
}