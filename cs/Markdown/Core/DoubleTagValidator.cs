using Markdown.Core.Tags;

namespace Markdown.Core
{
    public static class DoubleTagValidator
    {
        public static bool IsPossibleClosingTag(string line, int index, IDoubleTag tag)
        {
            return (IsLastIndexPossibleForTag(line, index, tag) || SymbolAfterTagIsSpace(line, index, tag)) &&
                   (IsFirstSymbol(index) || !SymbolBeforeCurrentIsSpace(line, index));
        }

        public static bool IsPossibleOpeningTag(string line, int index, IDoubleTag tag)
        {
            return (IsFirstSymbol(index) || SymbolBeforeCurrentIsSpace(line, index)) &&
                   (IsLastIndexPossibleForTag(line, index, tag) || !SymbolAfterTagIsSpace(line, index, tag));
        }

        public static bool TagStartsFromPosition(string line, int index, string tagValue) =>
            tagValue.Length + index <= line.Length && tagValue == line.Substring(index, tagValue.Length);

        private static bool IsFirstSymbol(int index) => index == 0;
        private static bool SymbolBeforeCurrentIsSpace(string line, int index) => char.IsWhiteSpace(line[index - 1]);

        private static bool SymbolAfterTagIsSpace(string line, int index, IDoubleTag tag) =>
            char.IsWhiteSpace(line[index + tag.Opening.Length]);

        private static bool IsLastIndexPossibleForTag(string line, int index, IDoubleTag tag) =>
            index == line.Length - tag.Closing.Length;
    }
}