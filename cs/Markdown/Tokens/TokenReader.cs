using System.Collections.Generic;
using System.Linq;

namespace Markdown.Tokens
{
    public class TokenReader
    {
        private readonly string text;
        private readonly Stack<Token> separatorStack;
        private int position;

        public TokenReader(string text)
        {
            this.text = text;
            separatorStack = new Stack<Token>();
        }

        public IEnumerable<Token> ReadAllTokens(ISeparatorHandler separatorHandler)
        {
            foreach (var currentSeparator in GetSeparators(separatorHandler))
            {
                if (SeparatorIsClosing(currentSeparator.TokenValue))
                {
                    var lastSeparatorPosition = separatorStack.Pop().Position;
                    yield return TwoSeparatorToken.FromSeparatorPositions(text, lastSeparatorPosition,
                        currentSeparator.Position,
                        currentSeparator.TokenValue);
                }
                else
                {
                    if (!SeparatorIsNested(currentSeparator.TokenValue) && position < currentSeparator.Position)
                    {
                        yield return new Token(position, currentSeparator.Position - position, text);
                    }
                    separatorStack.Push(currentSeparator);
                }

                UpdatePosition(currentSeparator);
            }

            if (position < text.Length)
                yield return new Token(position, text.Length - position, text);
            Reset();
        }

        private IEnumerable<Token> GetSeparators(ISeparatorHandler separatorHandler)
        {
            return Enumerable.Range(0, text.Length)
                .Where(i => separatorHandler.IsSeparator(text, i))
                .Select(i => new Token(i, separatorHandler.GetSeparatorLength(text, i), text))
                .Where(t => separatorHandler.IsSeparatorValid(text, t.Position, !SeparatorIsClosing(t.TokenValue)));
        }

        private bool SeparatorIsClosing(string separator)
        {
            return separatorStack.Count > 0 && separatorStack.Peek().TokenValue == separator;
        }

        private bool SeparatorIsNested(string separator)
        {
            return separatorStack.Count > 0 && separatorStack.Peek().TokenValue != separator;
        }

        private void UpdatePosition(Token separator)
        {
            position = SeparatorIsClosing(separator.TokenValue) ? separator.EndPosition :
                SeparatorIsNested(separator.TokenValue) ? position : position + separator.Position;
        }

        private void Reset()
        {
            separatorStack.Clear();
            position = 0;
        }
    }
}