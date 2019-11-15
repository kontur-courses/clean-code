using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkDown.TokenParsers
{
    public abstract class TokenParser
    {
        public abstract List<TokenType> ShieldingTokens { get; }
        public abstract TokenType Type { get; }
        public abstract (string from, string to) OpeningTags { get; }
        public abstract (string from, string to) ClosingTags { get; }


        public bool TokenShielded(List<TokenType> currentTokens) => currentTokens.Any(t => ShieldingTokens.Contains(t));
        public Token GetToken(string line, int startIndex)
        {
            if (startIndex + OpeningTags.from.Length > line.Length ||
                line.Substring(startIndex, OpeningTags.from.Length) != OpeningTags.from ||
                TagShielded(line, startIndex, true))
                return null;

            var startText = startIndex + OpeningTags.from.Length;
            var finalLine = new StringBuilder();
            finalLine.Append(OpeningTags.to);
            for (int i = startText; i < line.Length - ClosingTags.from.Length + 1; i++)
            {
                if (line.Substring(i, ClosingTags.from.Length) == ClosingTags.from && !TagShielded(line, i, false))
                {
                    if (startIndex + 1 == i)
                        return null;
                    finalLine.Append(line.Substring(startText, i - startText));
                    finalLine.Append(ClosingTags.to);
                    return new Token(finalLine.ToString(), startIndex,
                        i + ClosingTags.from.Length - startIndex, Type);
                }
            }

            return null;
        }

        public bool TryToFIndTag(string line, int startIndex, bool tagOpened)
        {
            return tagOpened
                ? startIndex + OpeningTags.from.Length < line.Length &&
                  line.Substring(startIndex, OpeningTags.from.Length) == OpeningTags.from &&
                  !TagShielded(line, startIndex, true)
                : startIndex + OpeningTags.from.Length < line.Length &&
                  line.Substring(startIndex, ClosingTags.from.Length) == ClosingTags.from &&
                  !TagShielded(line, startIndex, false);
        }
        private bool TagShielded(string line, int startIndex, bool tagOpened)
        {
            if (startIndex < 0 || startIndex > line.Length) throw new ArgumentException();

            var leftIndex = startIndex - 1;
            var rightIndex = startIndex + (tagOpened ? OpeningTags.from.Length : ClosingTags.from.Length);

            var shieldedLeft = leftIndex > 1 && TagShieldedFromLeft(line[leftIndex], tagOpened);
            var shieldedRight = line.Length > rightIndex && TagShieldedFromRight(line[rightIndex], tagOpened);
            return shieldedLeft || shieldedRight;
        }
        private bool TagShieldedFromLeft(char character, bool tagOpened) => character == '/' ||
                                                                           CharShielder(character);
        private bool TagShieldedFromRight(char character, bool tagOpened) => CharShielder(character) || (tagOpened && char.IsSeparator(character));


        private bool CharShielder(char character) =>
        char.IsDigit(character) || character == '_';

    }
}
