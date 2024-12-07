using Markdown.Domain.Tags;
using Markdown.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Checker
{
    public class TagChecker : ITagChecker
    {
        private int currentIndex;

        public const char HASH_SYMBOL = '#';
        public const char UNDERSCORE_SYMBOL = '_';
        public const char SLASH_SYMBOL = '\\';

        public Tuple<Tag, int> GetTagType(string line, int index)
        {
            currentIndex = index;
            var tag = Tag.None;

            switch (line[index])
            {
                case UNDERSCORE_SYMBOL:
                    tag = GetTagForUnderscore(line);
                    break;
                case SLASH_SYMBOL:
                    tag = GetTagForSlash(line);
                    break;
                case HASH_SYMBOL:
                    if (index == 0)
                        tag = Tag.Header;
                    break;
            }

            return Tuple.Create(tag, currentIndex);
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

            return Tag.None;
        }

        private Tag GetTagForUnderscore(string line)
        {
            if (currentIndex < line.Length - 1 && line[currentIndex + 1] == UNDERSCORE_SYMBOL)
                return GetTagWithMultipleUnderscores(line);

            return Tag.Italic;
        }

        private Tag GetTagWithMultipleUnderscores(string line)
        {
            if (currentIndex < line.Length - 2 && line[currentIndex + 2] == UNDERSCORE_SYMBOL)
            {
                currentIndex = FindEndOfInvalidTag(line);
                return Tag.None;
            }

            currentIndex++;

            return Tag.Bold;
        }

        private int FindEndOfInvalidTag(string line)
        {
            var endIndex = currentIndex;

            while (endIndex < line.Length && line[endIndex] == UNDERSCORE_SYMBOL)
                endIndex++;

            return endIndex;
        }
    }
}
