using Markdown.Tags;

namespace Markdown.Analyzer
{
    public class TagAnalyzer : ITagAnalyzer
    {
        private int currentIndex;
        public const char HASH_SYMBOL = '#';
        public const char UNDERSCORE_SYMBOL = '_';
        public const char SLASH_SYMBOL = '\\';

        public int Index => currentIndex;

        public (Tag, int) GetTagTypeWithIndex(string line, int index)
        {
            currentIndex = index;
            var tagType = Tag.NotATag;

            if (index == 0 && line.StartsWith(HASH_SYMBOL + " "))
            {
                tagType = Tag.Header;
            }
            if (line[index] == UNDERSCORE_SYMBOL)
            {
                tagType = GetTagForUnderscore(line);
            }
            else if (line[index] == SLASH_SYMBOL)
            {
                tagType = GetTagForSlash(line);
            }

            return (tagType, currentIndex);
        }

        private Tag GetTagForSlash(string line)
        {
            if (currentIndex < line.Length - 1
                && (line[currentIndex + 1] == SLASH_SYMBOL
                || line[currentIndex + 1] == UNDERSCORE_SYMBOL
                || line[currentIndex + 1] == HASH_SYMBOL))
            {
                return Tag.EscapedSymbol;
            }

            return Tag.NotATag;
        }
        private Tag GetTagForUnderscore(string line)
        {
            var intendedTagType = Tag.NotATag;

            if (currentIndex < line.Length - 1 && line[currentIndex + 1] == UNDERSCORE_SYMBOL)
            {
                intendedTagType = DefineTagWithMultipleUnderscores(line);
            }
            else
            {
                intendedTagType = Tag.Italic;
            }

            return intendedTagType;
        }

        private Tag DefineTagWithMultipleUnderscores(string line)
        {
            if (currentIndex < line.Length - 2 && line[currentIndex + 2] == UNDERSCORE_SYMBOL)
            {
                currentIndex = FindEndOfInvalidTag(line);
                return Tag.NotATag;
            }
            currentIndex++;

            return Tag.Bold;
        }

        private int FindEndOfInvalidTag(string line)
        {
            var endIndex = currentIndex;

            while (endIndex < line.Length && line[endIndex] == UNDERSCORE_SYMBOL)
            {
                endIndex++;
            }

            return endIndex;
        }
    }
}
