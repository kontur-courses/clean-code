using Markdown.Analyzer;
using Markdown.Tags;

namespace Markdown.TokenSearcher
{
    public class MarkdownTokenSearcher : ITokenSearcher
    {
        private ITagAnalyzer tagAnalyzer;
        private Stack<MdTag> needClosingTags;
        private Queue<Tag> offsetTags;
        private Dictionary<Tag, Tag> differentTagTypes = MarkdownConfig.DifferentTags;
        private Dictionary<Tag, string> mdTags = MarkdownConfig.MdTags;
        private int currentIndex;

        public MarkdownTokenSearcher(ITagAnalyzer tokenAnalyzer)
        {
            this.tagAnalyzer = tokenAnalyzer;
        }

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
            for (; currentIndex < line.Length; currentIndex++)
            {
                var tagTypeIndexPair = tagAnalyzer.GetTagTypeWithIndex(line, currentIndex);
                currentIndex = tagTypeIndexPair.Item2;
                AnalyzeTag(line, tagTypeIndexPair.Item1, fountedTokens);
            }
        }

        private void AnalyzeTag(string line, Tag tagType, List<Token> fountedTokens)
        {
            if (tagType == Tag.Header)
            {
                fountedTokens.Add(new Token(Tag.Header, 0, line.Length - 1));
            }
            else if (tagType == Tag.EscapedSymbol)
            {
                fountedTokens.Add(new Token(Tag.EscapedSymbol, currentIndex, currentIndex));
                currentIndex += 1;
            }
            else if (tagType != Tag.NotATag)
            {
                TryAddToken(tagType, line, fountedTokens);
            }
        }

        private void TryAddToken(Tag tagType, string line, List<Token> fountedTokens)
        {
            var openingTag = FindOpeningTag(tagType, currentIndex);

            if (openingTag.Tag == Tag.NotATag)
            {
                HandleNotATag(tagType, line);
            }
            else
            {
                HandleExistingTag(tagType, line, fountedTokens, openingTag);
            }
        }

        private void HandleExistingTag(Tag tagType, string line, List<Token> fountedTokens, MdTag openingTag)
        {
            var token = new Token(tagType, openingTag.Index, currentIndex);

            if (IsPossibleToAdd(token, line))
            {
                fountedTokens.Add(token);
            }
            else if (offsetTags.Count > 0 && offsetTags.Peek() == tagType)
            {
                needClosingTags.Push(new MdTag(tagType, currentIndex));
                offsetTags.Dequeue();
            }
        }

        private void HandleNotATag(Tag tagType, string line)
        {
            if (currentIndex < line.Length - 1 && !char.IsWhiteSpace(line[currentIndex + 1]))
            {
                needClosingTags.Push(new MdTag(tagType, currentIndex));
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
