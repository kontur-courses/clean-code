using Markdown.Tags;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http.Headers;

namespace Markdown.TokenSearcher
{
    public class MarkdownTokenSearcher : ITokenSearcher
    {
        private Stack<MdTag> needClosingTags;
        private Queue<TagType> offsetTags;
        private Dictionary<TagType, TagType> differentTagTypes = MarkdownConfig.DifferentTagTypes;
        private Dictionary<TagType, string> mdTags = MarkdownConfig.MdTags;


        public List<Token> SearchTokens(string markdownText)
        {
            var lines = markdownText.Split("\n");
            var fountedTokens = new List<Token>();

            foreach (var line in lines)
            {
                needClosingTags = new Stack<MdTag>();
                offsetTags = new Queue<TagType>();
                SearchTokensInLine(line, fountedTokens);
            }
            return fountedTokens;
        }

        private void SearchTokensInLine(string line, List<Token> fountedTokens)
        {
            if (line.StartsWith("# "))
                fountedTokens.Add(new Token(TagType.Header, 0, line.Length - 1));
            for (int i = 0; i < line.Length; i++)
            {
                switch (line[i])
                {
                    case '_':
                        var intendedTagType = TagType.NotATag;
                        if (i < line.Length - 1 && line[i + 1] == '_')
                        {
                            if (i < line.Length - 2 && line[i + 2] == '_')
                            {
                                i = FindEndOfInvalidTag(line, i);
                                intendedTagType = TagType.NotATag;
                            }
                            else
                            {
                                intendedTagType = TagType.Bold;
                                i++;
                            }
                        }
                        else
                            intendedTagType = TagType.Italic;
                        if (intendedTagType != TagType.NotATag)
                            TryAddToken(intendedTagType, i, line, fountedTokens);
                        break;
                    case '\\':
                        if (i < line.Length - 1 && (line[i + 1] == '\\' || line[i + 1] == '_' || line[i + 1] == '#'))
                        {
                            fountedTokens.Add(new Token(TagType.EscapedSymbol, i, i));
                            i++;
                        }
                        break;
                }
            }
        }

        private int FindEndOfInvalidTag(string line, int index) 
        {
            var endIndex = index;
            while (endIndex < line.Length && line[endIndex] == '_')
                endIndex++;
            return endIndex;
        }

        private void TryAddToken(TagType tagType, int index, string line, List<Token> fountedTokens)
        {
            var openingTag = FindOpeningTag(tagType, index);

            if (openingTag.Type == TagType.NotATag)
            {
                if (index < line.Length - 1 && !char.IsWhiteSpace(line[index + 1]))
                    needClosingTags.Push(new MdTag(tagType, index));
            }
            else
            {
                var token = new Token(tagType, openingTag.Index, index);
                if (IsPossibleToAdd(token, line))
                    fountedTokens.Add(token);
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
                return false;

            if (offsetTags.Dequeue() == differentTagTypes[token.TagType])
                return false;

            if (token.EndIndex < line.Length - 1 && !char.IsWhiteSpace(line[token.EndIndex + 1]) && subString.Any(char.IsWhiteSpace))
                return false;

            if (token.TagType == TagType.Bold && needClosingTags.Any(tag => tag.Type == differentTagTypes[token.TagType]))
                return false;

            if (char.IsWhiteSpace(subString[0]) || char.IsWhiteSpace(subString[subString.Length - 1]))
                return false;

            if (token.StartIndex - 1 > 0 && !char.IsWhiteSpace(line[token.StartIndex - shift]) && subString.Any(char.IsWhiteSpace))
                return false;

            return true;
        }

        private MdTag FindOpeningTag(TagType tagType, int index)
        {
            var openingTag = new MdTag(TagType.NotATag, index);

            while (needClosingTags.Any(tag => tag.Type == tagType))
            {
                var removeClosingTag = needClosingTags.Pop();
                openingTag = new MdTag(removeClosingTag.Type, removeClosingTag.Index);
                offsetTags.Enqueue(removeClosingTag.Type);
            }

            return openingTag;
        }
    }
}
