using System.Collections.Generic;
using System.Linq;
using Markdown.Tags;

namespace Markdown
{
    internal static class TagsParser
    {
        private static List<TagToken> ParseAllMarkdownTagTokens(string paragraph)
        {
            var tagTokens = new List<TagToken>();
            for (var i = 0; i < paragraph.Length; i++)
                if (IsNotShieldedTag(paragraph, i))
                    if (i + 1 < paragraph.Length && paragraph[i + 1] == paragraph[i])
                    {
                        tagTokens.Add(new TagToken(i, new string(paragraph[i], 2)));
                        i++;
                    }
                    else
                    {
                        tagTokens.Add(new TagToken(i, paragraph[i].ToString()));
                    }

            return tagTokens;
        }

        private static bool IsNotShieldedTag(string paragraph, int i)
        {
            return Tag.MdTagValues.Any(x => x.StartsWith(paragraph[i])) && (i < 1 || Enumerable.Range(0, i).Reverse()
                .TakeWhile(x => paragraph[x] == '\\').Count() % 2 == 0);
        }

        internal static List<Tag> GetCorrectTags(string text)
        {
            ICollection<TagToken> tokens = ParseAllMarkdownTagTokens(text);
            return GetCorrectTags(tokens, text);
        }

        private static List<Tag> GetCorrectTags(ICollection<TagToken> tokens, string text)
        {
            var tags = new List<Tag>();
            if (tokens == null || tokens.Count == 0)
                return new List<Tag>();
            if (tokens.First().Tag == MdTagsEnum.Header)
                tags.Add(new SingleTag(MdTagsEnum.Header, tokens.First().StartPosition));
            tags.AddRange(GetTags(tokens));
            Md.RemoveIncorrectTags(text, tags);
            return tags;
        }

        private static List<(TagToken, TagToken)> GetCorrectPairsTokens(ICollection<TagToken> tokens)
        {
            var pairs = new List<(TagToken, TagToken)>();
            var tokensStack = new Stack<TagToken>();
            foreach (var token in tokens.Where(x => x.Tag != MdTagsEnum.Header).ToList())
                if (tokensStack.Count > 0 && token.Tag == tokensStack.Peek().Tag)
                    if (token.Tag != MdTagsEnum.Strong || tokensStack.All(x => x.Tag != MdTagsEnum.Italic))
                        pairs.Add((tokensStack.Pop(), token));
                    else
                        tokensStack.Pop();
                else
                    tokensStack.Push(token);
            return pairs;
        }

        private static List<Tag> GetTags(ICollection<TagToken> tokens)
        {
            var correctTokens = new List<Tag>();
            foreach (var (opening, closing) in GetCorrectPairsTokens(tokens))
            {
                var close = new ClosingTag(closing.Tag, closing.StartPosition);
                correctTokens.Add(new OpeningTag(opening.Tag, opening.StartPosition, close));
                close.PairTag = (OpeningTag) correctTokens.Last();
                correctTokens.Add(close);
            }

            return correctTokens;
        }
    }
}