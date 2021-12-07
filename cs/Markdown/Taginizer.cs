using Markdown.TagEvents;
using System;
using System.Collections.Generic;
using Markdown.TagParsers;

namespace Markdown
{
    public class Taginizer
    {
        private readonly List<TagEvent> tokens;
        private readonly string input;

        public Taginizer(string input)
        {
            tokens = new List<TagEvent>();
            this.input = input;
        }

        public List<TagEvent> Taginize()
        {
            for (var index = 0; index < input.Length; index++)
            {
                var symbol = input[index];
                if (symbol.IsLetterOrPunctuationButNotTag())
                {
                    index = AddSeveralSymbolsAsTokenAndGetNextIndex(s => s.IsLetterOrPunctuationButNotTag(),
                        TagName.Word, index);
                    continue;
                }

                if (char.IsDigit(symbol))
                {
                    index = AddSeveralSymbolsAsTokenAndGetNextIndex(char.IsDigit,
                        TagName.Number, index);
                    continue;
                }

                if (symbol.IsWhitespaceButNotNewLine())
                {
                    index = AddSeveralSymbolsAsTokenAndGetNextIndex(s => s.IsWhitespaceButNotNewLine(),
                        TagName.Whitespace, index);
                    continue;
                }
                switch (symbol)
                {
                    case '_':
                        index = AddUnderlinersTagAndGetNextIndex(index);
                        break;
                    case '#':
                        index = AddHashtagAndGetNextIndex(index);
                        break;
                    case '\n':
                        tokens.Add(new TagEvent(TagSide.Right, TagName.NewLine, symbol.ToString()));
                        break;
                    case '\\':
                        tokens.Add(new TagEvent(TagSide.None, TagName.Escape, symbol.ToString()));
                        break;
                    default:
                        tokens.Add(new TagEvent(TagSide.None, TagName.Word, symbol.ToString()));
                        break;
                }
            }
            tokens.Add(new TagEvent(TagSide.None, TagName.Eof, ""));
            return tokens;
        }

        private int AddSeveralSymbolsAsTokenAndGetNextIndex(
            Func<char, bool> symbolSelector,
            TagName tagName, 
            int symbolIndex)
        {
            var contentLength = GetTagContentLength(symbolIndex, symbolSelector);
            var tagContent = input.Substring(symbolIndex, contentLength);
            tokens.Add(new TagEvent(TagSide.None, tagName, tagContent));
            return symbolIndex + contentLength - 1;
        }
        private int AddHashtagAndGetNextIndex(int index)
        {
            if (IsSymbolMeansHeader(index))
            {
                tokens.Add(new TagEvent(TagSide.Left, TagName.Header, "# "));
                return index + 1;
            }

            return AddSeveralSymbolsAsTokenAndGetNextIndex(s => s == '#', TagName.Word, index);
        }

        private bool IsSymbolMeansHeader(int index)
        {
            return (IsHashtagFirstSymbol(index) 
                    || IsHashtagGoesAfterNewLine(index) 
                    || IsHashtagGoesAfterSlash(index))
                   && IsNextSymbolWhiteSpace(index);
        }

        private bool IsHashtagGoesAfterSlash(int index)
        {
            return input[index - 1] == '\\';
        }

        private bool IsHashtagGoesAfterNewLine(int index)
        {
            return input[index - 1] == '\n';
        }

        private static bool IsHashtagFirstSymbol(int index)
        {
            return index == 0;
        }

        private bool IsNextSymbolWhiteSpace(int index)
        {
            var nextInd = index + 1;
            return nextInd < input.Length && input[nextInd] == ' ';
        }

        private int AddUnderlinersTagAndGetNextIndex(int index)
        {
            var nextIndex = index + 1;
            if (IsDoubleUnderlineDetected(nextIndex))
            {
                tokens.Add(new TagEvent(TagSide.None, TagName.DoubleUnderliner, "__"));
                index = nextIndex;
            }
            else
                tokens.Add(new TagEvent(TagSide.None, TagName.Underliner, "_"));

            return index;
        }

        private bool IsDoubleUnderlineDetected(int nextIndex)
        {
            return nextIndex < input.Length && input[nextIndex] == '_';
        }

        private int GetTagContentLength(int index, Func<char, bool> predicate)
        {
            var length = 0;
            while (index < input.Length && predicate(input[index]))
            {
                length++;
                index++;
            }
            return length;
        }
    }
}
