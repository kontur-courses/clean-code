using Markdown.Tags;

namespace Markdown.TokenSearcher
{
    public class MarkdownTokenSearcher : ITokenSearcher
    {
        private Stack<MdTag> needClosingTags;
        private Queue<Tag> offsetTags;
        private Dictionary<Tag, Tag> differentTagTypes = MarkdownConfig.DifferentTags;
        private Dictionary<Tag, string> mdTags = MarkdownConfig.MdTags;
        private int currentIndex;

        private const char HASH_SYMBOL = '#';
        private const char Underscore_SYMBOL = '_';
        private const char SLASH_SYMBOL = '\\';

        public List<Token> SearchTokens(string markdownText)
        {
            var lines = markdownText.Split("\n");
            var fountedTokens = new List<Token>();

            foreach (var line in lines)
            {
                needClosingTags = new Stack<MdTag>();
                offsetTags = new Queue<Tag>();
                currentIndex = 0;
                SearchTokensInLine(line, fountedTokens);
            }

            return fountedTokens;
        }

        private void SearchTokensInLine(string line, List<Token> fountedTokens)
        {
            TryAddHeaderToken(line, fountedTokens);
            for (; currentIndex < line.Length; currentIndex++)
            {
                AnalyzeSymbol(line, fountedTokens);
            }
        }

        private void TryAddHeaderToken(string line, List<Token> fountedTokens)
        {
            if (line.StartsWith(HASH_SYMBOL + " "))
            {
                fountedTokens.Add(new Token(Tag.Header, 0, line.Length - 1));
            }
        }

        private void AnalyzeSymbol(string line, List<Token> fountedTokens)
        {
            if (line[currentIndex] == Underscore_SYMBOL)
            {
                AnalyzeUnderscore(line, currentIndex, fountedTokens);
            }
            else if (line[currentIndex] == SLASH_SYMBOL)
            {
                AnalyzeEscapeSequence(line, currentIndex, fountedTokens);
            }
        }

        private void AnalyzeUnderscore(string line, int index, List<Token> fountedTokens)
        {
            var intendedTagType = Tag.NotATag;

            if (index < line.Length - 1 && line[index + 1] == Underscore_SYMBOL)
            {
                intendedTagType = DefineTagWithMultipleUnderscores(line, index);
            }
            else
            {
                intendedTagType = Tag.Italic;
            }

            if (intendedTagType != Tag.NotATag)
            {
                TryAddToken(intendedTagType, currentIndex, line, fountedTokens);
            }
        }

        private Tag DefineTagWithMultipleUnderscores(string line, int index)
        {
            if (index < line.Length - 2 && line[index + 2] == Underscore_SYMBOL)
            {
                currentIndex = FindEndOfInvalidTag(line, index);
                return Tag.NotATag;
            }
            currentIndex++;

            return Tag.Bold;

        }

        private void AnalyzeEscapeSequence(string line, int index, List<Token> fountedTokens)
        {
            if (index < line.Length - 1 && (line[index + 1] == SLASH_SYMBOL || line[index + 1] == Underscore_SYMBOL || line[index + 1] == HASH_SYMBOL))
            {
                fountedTokens.Add(new Token(Tag.EscapedSymbol, index, index));
                currentIndex = index + 1;
            }
        }

        private int FindEndOfInvalidTag(string line, int index) 
        {
            var endIndex = index;

            while (endIndex < line.Length && line[endIndex] == Underscore_SYMBOL)
            {
                endIndex++;
            }

            return endIndex;
        }

        private void TryAddToken(Tag tagType, int index, string line, List<Token> fountedTokens)
        {
            var openingTag = FindOpeningTag(tagType, index);

            if (openingTag.Tag == Tag.NotATag)
            {
                if (index < line.Length - 1 && !char.IsWhiteSpace(line[index + 1]))
                {
                    needClosingTags.Push(new MdTag(tagType, index));
                }
            }
            else
            {
                var token = new Token(tagType, openingTag.Index, index);
                if (IsPossibleToAdd(token, line))
                {
                    fountedTokens.Add(token);
                }
                else if (offsetTags.Count > 0 && offsetTags.Peek() == tagType)
                {
                    needClosingTags.Push(new MdTag(tagType, index));
                    offsetTags.Dequeue();
                }
            }
        }

        private bool IsPossibleToAdd(Token token, string line)
        {
            var subString = line.Substring(token.StartIndex + 1, token.EndIndex - token.StartIndex - 1);
            var shift = mdTags[token.TagType].Length;

            if (char.IsWhiteSpace(line[token.EndIndex - shift]))
            {
                return false;
            }

            if (offsetTags.Dequeue() == differentTagTypes[token.TagType])
            {
                return false;
            }

            if (token.EndIndex < line.Length - 1 && !char.IsWhiteSpace(line[token.EndIndex + 1]) && subString.Any(char.IsWhiteSpace))
            {
                return false;
            }

            if (token.TagType == Tag.Bold && needClosingTags.Any(tag => tag.Tag == differentTagTypes[token.TagType]))
            {
                return false;
            }

            if (char.IsWhiteSpace(subString[0]) || char.IsWhiteSpace(subString[subString.Length - 1]))
            {
                return false;
            }

            if (token.StartIndex - 1 > 0 && !char.IsWhiteSpace(line[token.StartIndex - shift]) && subString.Any(char.IsWhiteSpace))
            {
                return false;
            }

            return true;
        }

        private MdTag FindOpeningTag(Tag tagType, int index)
        {
            var openingTag = new MdTag(Tag.NotATag, index);

            while (needClosingTags.Any(tag => tag.Tag == tagType))
            {
                var removeClosingTag = needClosingTags.Pop();
                openingTag = new MdTag(removeClosingTag.Tag, removeClosingTag.Index);
                offsetTags.Enqueue(removeClosingTag.Tag);
            }

            return openingTag;
        }
    }
}
