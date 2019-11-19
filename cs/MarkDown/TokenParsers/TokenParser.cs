using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkDown.TokenParsers
{
    public abstract class TokenParser
    {
        public abstract List<TokenType> EscapingTokens { get; }
        public abstract TokenType Type { get; }
        public abstract (string from, string to) OpeningTags { get; }
        public abstract (string from, string to) ClosingTags { get; }


        public bool TokenEscaped(List<TokenType> currentTokens) => currentTokens.Any(t => EscapingTokens.Contains(t));

        public StringBuilder GetTokenParsed(string line, Token token)
        {
            if (Type != token.Type) throw new ArgumentException("Cant parse that Token Type");
            var builder = new StringBuilder();
            builder.Append(OpeningTags.to);
            builder.Append(line.Substring(token.openTag.indexNextToTag, token.ValueLength));
            builder.Append(ClosingTags.to);
            return builder;
        }

        public bool IsTag(string line, int startIndex, bool tagOpened)
        {
            return tagOpened
                ? startIndex + OpeningTags.from.Length <= line.Length &&
                  line.Substring(startIndex, OpeningTags.from.Length) == OpeningTags.from &&
                  !TagEscaped(line, startIndex, true)
                : startIndex + ClosingTags.from.Length <= line.Length &&
                  line.Substring(startIndex, ClosingTags.from.Length) == ClosingTags.from &&
                  !TagEscaped(line, startIndex, false);
        }

        private bool TagEscaped(string line, int startIndex, bool tagOpened)
        {
            if (startIndex < 0 || startIndex > line.Length) throw new ArgumentException();

            var leftIndex = startIndex - 1;
            var rightIndex = startIndex + (tagOpened ? OpeningTags.from.Length : ClosingTags.from.Length);

            var escapedLeft = leftIndex >= 0 && TagEscapedFromLeft(line, leftIndex, tagOpened);
            var escapedRight = line.Length > rightIndex && TagEscapedFromRight(line, rightIndex, tagOpened);
            return escapedLeft || escapedRight;
        }

        private bool TagEscapedFromLeft(string line, int charIndex, bool tagOpened) => line[charIndex] == '\\' ||
                                                                                       CharIsEscaped(line, charIndex);

        private bool TagEscapedFromRight(string line, int charIndex, bool tagOpened) =>
            CharIsEscaped(line, charIndex) || (tagOpened && char.IsSeparator(line[charIndex]));


        private bool CharIsEscaped(string line, int charIndex) =>
            char.IsDigit(line[charIndex]) || (line[charIndex] == '_' && (charIndex <= 0 || line[charIndex-1]!='\\'));
    }
}