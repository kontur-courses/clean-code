using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Tokens
{
    public class TokenReader
    {
        private readonly string text;
        private readonly Stack<Token> separatorStack;

        public TokenReader(string text)
        {
            this.text = text;
            separatorStack = new Stack<Token>();
        }

        public IEnumerable<TwoSeparatorToken> ReadAllTwoSeparatorTokens(Func<string, int, bool> isSeparator,
            Func<string, int, string> getSeparator, Func<string, int, bool, bool> isSeparatorValid)
        {
            foreach (var separatorPos in Enumerable.Range(0, text.Length).Where(i => isSeparator(text, i)))
            {
                var currentSeparator = getSeparator(text, separatorPos);
                if (!isSeparatorValid(text, separatorPos, !SeparatorIsClosing(currentSeparator)))
                    continue;
                if (SeparatorIsClosing(currentSeparator))
                {
                    var lastSeparatorPosition = separatorStack.Pop().Position;
                    yield return TwoSeparatorToken.FromSeparatorPositions(text, lastSeparatorPosition, separatorPos,
                        currentSeparator);
                }
                else
                    separatorStack.Push(new Token(separatorPos, currentSeparator));
            }
        }

        private bool SeparatorIsClosing(string separator)
        {
            return separatorStack.Count > 0 && separatorStack.Peek().Value == separator;
        }
    }
}