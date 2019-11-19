using Markdown.Core.Tags;

namespace Markdown.Core
{
    static class TagValidator
    {
        private static bool IsFirstSymbol(int index) => index == 0;
        private static bool SymbolBeforeCurrentIsSpace(string line, int index) => char.IsWhiteSpace(line[index - 1]);

        private static bool SymbolAfterTagIsSpace(string line, int index, ITag tag) =>
            char.IsWhiteSpace(line[index + tag.Opening.Length]);

        private static bool IsLastIndexPossibleForTag(string line, int index, ITag tag) =>
            index == line.Length - tag.Closing.Length;

        public static bool IsPossibleClosingTag(string line, int index, ITag tag)
        {
            return (IsLastIndexPossibleForTag(line, index, tag) || SymbolAfterTagIsSpace(line, index, tag)) &&
                   (IsFirstSymbol(index) || !SymbolBeforeCurrentIsSpace(line, index));
        }

        public static bool IsPossibleOpeningTag(string line, int index, ITag tag)
        {
            return (IsFirstSymbol(index) || SymbolBeforeCurrentIsSpace(line, index)) &&
                   (IsLastIndexPossibleForTag(line, index, tag) || !SymbolAfterTagIsSpace(line, index, tag));
        }
    }
}