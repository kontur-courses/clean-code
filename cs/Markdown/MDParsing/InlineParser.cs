using Markdown.Abstractions;
using Markdown.MDElements;
using Markdown.MDNodes; 

namespace Markdown.Parsing
{
    public class InlineParser
    {
        private readonly List<Token> _tokens;
        private int _currentIndex;

        public InlineParser(List<Token> tokens)
        {
            _tokens = tokens;
            _currentIndex = 0;
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

            // 1. Если это текст
            if (token.UnderscoreCount == 0)
            {
                _currentIndex++;
                return new TextNode(token.Text);
            }

            if (token.UnderscoreCount >= 2)
            {
                var node = TryParsePaired(2, "__", children => new StrongNode(children));
                if (node != null) return node;
            }

            if (token.UnderscoreCount >= 1)
            {
                var node = TryParsePaired(1, "_", children => new ItalicNode(children));
                if (node != null) return node;
            }

            // 4. Если ничего не подошло, возвращаем как текст
            _currentIndex++;
            return new TextNode(new string('_', token.UnderscoreCount));
        }


        private InlineNode TryParsePaired(int count, string delimiter, Func<List<InlineNode>, InlineNode> factory)
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

                if (current.UnderscoreCount >= count)
                {
                    int remainingFromClose = current.UnderscoreCount - count;
                    isClosed = true;
                    _currentIndex++;

                    if (remainingFromOpen > 0)
                    {
                        innerTokens.Insert(0, new Token { UnderscoreCount = remainingFromOpen });
                    }

                    if (remainingFromClose > 0)
                    {
                        innerTokens.Add(new Token { UnderscoreCount = remainingFromClose });
                    }

                    break;
                }

                innerTokens.Add(new Token
                {
                    Text = current.Text,
                    UnderscoreCount = current.UnderscoreCount
                });
                _currentIndex++;
            }

            if (isClosed)
            {
                var innerTextForValidation = string.Join("", innerTokens.Select(t => t.Text ?? new string('_', t.UnderscoreCount)));
                if (string.IsNullOrWhiteSpace(innerTextForValidation))
                {
                    _currentIndex = savedIndex;
                    return null;
                }

                var innerParser = new InlineParser(innerTokens);
                var innerNodes = innerParser.ParseAll();

                return factory(innerNodes);
            }
            else
            {
                _currentIndex = savedIndex;
                return null;
            }
        }
    }
}