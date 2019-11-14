using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkDown.TokenDeclarations
{
    public abstract class TokenDeclaration
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
                TagShilded(line, startIndex, true))
                return null;

            var startText = startIndex + OpeningTags.from.Length;
            var finalLine = new StringBuilder();
            finalLine.Append(OpeningTags.to);
            for (int i = startText; i < line.Length - ClosingTags.from.Length + 1; i++)
            {
                if (line.Substring(i, ClosingTags.from.Length) == ClosingTags.from && !TagShilded(line, i, false))
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

        private bool TagShilded(string line, int startIndex, bool tagOpened)
        {
            if (startIndex < 0 || startIndex > line.Length) throw new ArgumentException();

            var leftIndex = startIndex - 1;
            var rightIndex = startIndex + (tagOpened ? OpeningTags.from.Length : ClosingTags.from.Length);

            var shildedLeft = leftIndex > 1 && TagShildedFromLeft(line[leftIndex], tagOpened);
            var shildedRight = line.Length > rightIndex && TagShildedFromRight(line[rightIndex], tagOpened);
            return shildedLeft || shildedRight;
        }
        //решарпер меняет на страшные непонятные выражения
        private bool TagShildedFromLeft(char character, bool tagOpened) => character == '/' ||
                                                                           CharShilder(character);
        private bool TagShildedFromRight(char character, bool tagOpened) => CharShilder(character)|| (tagOpened && char.IsSeparator(character) );


        private bool CharShilder(char character) =>
        char.IsDigit(character) || character == '_';

    }
}
