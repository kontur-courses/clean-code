using Markdown.Abstractions;
using Markdown.MDElements;
using Markdown.MDNodes;
using Markdown.MDParsing;
using System;
using System.Collections.Generic;

namespace Markdown.Parsing
{
    public class InlineParser
    {
        private readonly List<Token> _tokens;
        private int _currentIndex;
        private readonly bool _allowStrong;

        public InlineParser(List<Token> tokens, bool allowStrong = true)
        {
            _tokens = tokens;
            _currentIndex = 0;
            _allowStrong = allowStrong;
        }

        public List<InlineNode> ParseAll()
        {
            var nodes = new List<InlineNode>();
            while (_currentIndex < _tokens.Count)
            {
                var node = ParseNext();
                if (node != null) nodes.Add(node);
            }
            return nodes;
        }

        private InlineNode ParseNext()
        {
            if (_currentIndex >= _tokens.Count) return null;

            var token = _tokens[_currentIndex];

            if (token.UnderscoreCount == 0)
            {
                _currentIndex++;
                return new TextNode(token.Text);
            }

            if (!_allowStrong && token.UnderscoreCount >= 1)
            {
                _currentIndex++;
                return new TextNode(new string('_', token.UnderscoreCount));
            }

            if (token.UnderscoreCount >= 2)
            {
                var node = TryParsePaired(2, "__", children => new StrongNode(children));
                if (node != null) return node;
            }

            if (token.UnderscoreCount >= 1)
            {
                var node = TryParsePaired(1, "_", children => new ItalicNode(children), allowStrongInside: false);
                if (node != null) return node;
            }

            _currentIndex++;
            return new TextNode(new string('_', token.UnderscoreCount));
        }

        private InlineNode TryParsePaired(
            int count,
            string delimiter,
            Func<List<InlineNode>, InlineNode> factory,
            bool allowStrongInside = true)
        {
            int savedIndex = _currentIndex;
            int openingUnderscores = _tokens[_currentIndex].UnderscoreCount;
            int remainingFromOpen = openingUnderscores - count;

            _currentIndex++;

            var innerTokens = new List<Token>();
            bool isClosed = false;

            while (_currentIndex < _tokens.Count)
            {
                var current = _tokens[_currentIndex];

                bool isClosingToken = count == 2 ? current.UnderscoreCount >= 2 : current.UnderscoreCount == 1;

                if (isClosingToken)
                {
                    int remainingFromClose = current.UnderscoreCount - count;
                    isClosed = true;
                    _currentIndex++;

                    if (remainingFromOpen > 0)
                        innerTokens.Insert(0, new Token(underscoreCount: remainingFromOpen));

                    if (remainingFromClose > 0)
                        innerTokens.Add(new Token(underscoreCount: remainingFromClose));

                    break;
                }

                innerTokens.Add(current);
                _currentIndex++;
            }

            if (!isClosed)
            {
                _currentIndex = savedIndex;
                return null;
            }

            Token? tokenBeforeOpen = savedIndex > 0 ? _tokens[savedIndex - 1] : null;
            Token? tokenAfterClose = _currentIndex < _tokens.Count ? _tokens[_currentIndex] : null;

            if (!InlineTokenValidator.IsValid(tokenBeforeOpen, tokenAfterClose, innerTokens, count))
            {
                _currentIndex = savedIndex;
                return null;
            }

            var innerParser = new InlineParser(innerTokens, allowStrong: allowStrongInside);
            var innerNodes = innerParser.ParseAll();

            return factory(innerNodes);
        }
    }
}