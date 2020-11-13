using System.Collections.Generic;
using System.Linq;
using Markdown.Tags;

namespace Markdown
{
    public class Md
    {
        private static readonly List<char> beginsOfTags = new List<char> {'_', '#'};

        public static string Render(string text)
        {
            var paragraphs = TextWorker.SplitOnParagraphs(text);
            var res = paragraphs.Select(paragraph =>
                ReplaceMarkDownOnTags(text, GetCorrectTags(ParseAllMarkdownTokens(paragraph), text))).ToList();
            return string.Join("\n\r", res);
        }

        public static List<TagToken> ParseAllMarkdownTokens(string paragraph)
        {
            var tagTokens = new List<TagToken>();
            for (var i = 0; i < paragraph.Length; i++)
                if (beginsOfTags.Contains(paragraph[i]) && (i < 1 || Enumerable.Range(0, i).Reverse()
                    .TakeWhile(x => paragraph[x] == '\\').Count() % 2 == 0))
                {
                    if (i + 1 < paragraph.Length && paragraph[i + 1] == paragraph[i])
                    {
                        tagTokens.Add(new TagToken(i, new string(paragraph[i], 2)));
                        i++;
                    }
                    else
                        tagTokens.Add(new TagToken(i, paragraph[i].ToString()));
                }

            return tagTokens;
        }

        public static List<Tag> GetCorrectTags(ICollection<TagToken> tokens, string text)
        {
            var tags = new List<Tag>();
            if (tokens == null || tokens.Count == 0)
                return new List<Tag>();
            if (tokens.First().Tag == "#") tags.Add(new SingleTag("#", tokens.First().StartPosition));
            tags.AddRange(GetTags(tokens));
            RemoveIncorrectTags(text, tags);
            return tags;
        }

        private static List<Tag> RemoveIncorrectTags(string text, List<Tag> tags)
        {
            var iterator = tags.ToList();
            foreach (var tag in iterator)
                switch (tag)
                {
                    case OpeningTag _ when tag.position + 1 < text.Length && text[tag.position + 1] == ' ':
                    case ClosingTag _ when tag.position - 1 > 0 && text[tag.position - 1] == ' ':
                    case OpeningTag _ when EmptyContent(tag):
                        RemovePairTag(tags, tag);
                        break;
                    default:
                        if (TagsInWord(text, tag))
                        {
                            var (wordBegin, wordEnd) = GetBeginAndEndOfWord(text, tag);
                            if (text.Substring(wordBegin, wordEnd - wordBegin).Any(x => x == ' ' || char.IsDigit(x)))
                                RemovePairTag(tags, tag);
                        }

                        break;
                }

            return tags.Where(x => x != null).ToList();
        }

        private static (int wordBegin, int wordEnd) GetBeginAndEndOfWord(string text, Tag tag)
        {
            var wordBegin = tag.position;
            var wordEnd = tag.position;
            for (; wordBegin > tag.PairTag.position || wordBegin > 0 && text[wordBegin] != ' '; wordBegin--) ;
            for (; wordEnd < tag.PairTag.position || wordEnd < text.Length - 1 && text[wordEnd] != ' '; wordEnd++) ;
            return (text[wordBegin] == ' ' ? wordBegin + 1 : wordBegin, text[wordEnd] == ' ' ? wordEnd - 1 : wordEnd);
        }

        private static List<Tag> GetTags(ICollection<TagToken> tokens)
        {
            var correctTokens = new List<Tag>();
            foreach (var (opening, closing) in GetCorrectPairsTokens(tokens))
            {
                var close = new ClosingTag(closing.Tag, closing.StartPosition);
                correctTokens.Add(new OpeningTag(opening.Tag, opening.StartPosition, close));
                close.OpeningTag = (OpeningTag) correctTokens.Last();
                correctTokens.Add(close);
            }

            return correctTokens;
        }

        private static bool TagsInWord(string text, Tag tag) =>
            tag is OpeningTag && tag.position > 0 && text[tag.position - 1] != ' ' ||
            tag is ClosingTag && text.Length > tag.position + tag.mdTag.Length &&
            text[tag.position + tag.mdTag.Length] != ' ';

        private static bool EmptyContent(Tag tag) => tag.PairTag.position - tag.position - tag.mdTag.Length == 0;

        private static void RemovePairTag(List<Tag> correctTokens, Tag tag)
        {
            correctTokens.Remove(tag);
            correctTokens.Remove(tag.PairTag);
        }

        private static List<(TagToken, TagToken)> GetCorrectPairsTokens(ICollection<TagToken> tokens)
        {
            var pairs = new List<(TagToken, TagToken)>();
            var tokensStack = new Stack<TagToken>();
            foreach (var token in tokens.Where(x => x.Tag != "#").ToList())
                if (tokensStack.Count > 0 && token.Tag == tokensStack.Peek().Tag)
                    if (token.Tag != "__" || tokensStack.All(x => x.Tag != "_"))
                        pairs.Add((tokensStack.Pop(), token));
                    else
                        tokensStack.Pop();
                else
                    tokensStack.Push(token);
            return pairs;
        }

        private static string ReplaceMarkDownOnTags(string text, List<Tag> tags)
        {
            var header = tags.FirstOrDefault(x => x is SingleTag);
            if (header != null) tags.Remove(header);
            foreach (var tag in tags.OrderByDescending(x => x.position))
            {
                text = text.Remove(tag.position, tag.mdTag.Length);
                text = text.Insert(tag.position, tag.htmlTag);
            }

            if (header == null) return text;
            text = text.Remove(0, 1 + text.Skip(1).TakeWhile(x => x == ' ').Count());
            text = text.Insert(0, "<h1>");
            text = text.Insert(text.Length, "</h1>");

            return text;
        }
    }
}