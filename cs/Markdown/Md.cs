using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tags;

namespace Markdown
{
    public class Md
    {
        public static string Render(string text)
        {
            var paragraphs = TextWorker.SplitOnParagraphs(text);
            var formattedParagraphs = paragraphs.Select(paragraph =>
                FormatParagraph(text, paragraph)).ToList();
            return string.Join(Environment.NewLine, formattedParagraphs);
        }

        private static string FormatParagraph(string text, string paragraph)
        {
            return TextWorker.RemoveShieldsBeforeKeyChars(
                ReplaceMarkDownOnTags(text,
                    TagsParser.GetCorrectTags(text)),
                Tag.MdTagValues.Select(x => x[0]).Distinct().ToList());
        }

        internal static void RemoveIncorrectTags(string text, List<Tag> tags)
        {
            var iterator = tags.ToList();
            foreach (var tag in iterator)
                switch (tag)
                {
                    case OpeningTag when tag.position + 1 < text.Length && char.IsWhiteSpace(text[tag.position + 1]):
                    case ClosingTag when tag.position - 1 > 0 && char.IsWhiteSpace(text[tag.position - 1]):
                    case OpeningTag when EmptyContent(tag):
                        RemovePairTag(tags, tag);
                        break;
                    default:
                        if (TagsInWord(text, tag))
                        {
                            var (wordBegin, wordEnd) = GetBeginAndEndOfWord(text, tag);
                            if (text.Substring(wordBegin, wordEnd - wordBegin)
                                .Any(x => char.IsWhiteSpace(x) || char.IsDigit(x)))
                                RemovePairTag(tags, tag);
                        }

                        break;
                }
        }

        private static (int wordBegin, int wordEnd) GetBeginAndEndOfWord(string text, Tag tag)
        {
            var wordBegin = tag.position;
            var wordEnd = tag.position;
            for (;
                wordBegin > tag.PairTag.position || wordBegin > 0 && !char.IsWhiteSpace(text[wordBegin]);
                wordBegin--) ;
            for (;
                wordEnd < tag.PairTag.position || wordEnd < text.Length - 1 && !char.IsWhiteSpace(text[wordBegin]);
                wordEnd++) ;
            return (char.IsWhiteSpace(text[wordBegin]) ? wordBegin + 1 : wordBegin,
                char.IsWhiteSpace(text[wordBegin]) ? wordEnd - 1 : wordEnd);
        }


        private static bool TagsInWord(string text, Tag tag)
        {
            return tag is OpeningTag && tag.position > 0 && !char.IsWhiteSpace(text[tag.position - 1]) ||
                   tag is ClosingTag && text.Length > tag.position + tag.mdTag.Length &&
                   !char.IsWhiteSpace(text[tag.position + tag.mdTag.Length]);
        }

        private static bool EmptyContent(Tag tag)
        {
            return tag.PairTag.position - tag.position - tag.mdTag.Length == 0;
        }

        private static void RemovePairTag(List<Tag> correctTokens, Tag tag)
        {
            correctTokens.Remove(tag);
            correctTokens.Remove(tag.PairTag);
        }

        private static string ReplaceMarkDownOnTags(string text, List<Tag> tags)
        {
            var header = tags.FirstOrDefault(x => x is SingleTag);
            if (header != null)
            {
                tags.Remove(header);
                header = new OpeningTag(header.mdTag, header.position);
            }

            foreach (var tag in tags.OrderByDescending(x => x.position))
                text = ChangeMdTagOnHtml(text, tag);

            if (header == null) return text;
            text = text.Remove(0, text.Skip(1).TakeWhile(char.IsWhiteSpace).Count());
            text = ChangeMdTagOnHtml(text, header);
            text = text.Insert(text.Length, new ClosingTag(header.mdTag, text.Length - 1).htmlTag);

            return text;
        }

        private static string ChangeMdTagOnHtml(string text, Tag tag)
        {
            text = text.Remove(tag.position, tag.mdTag.Length);
            text = text.Insert(tag.position, tag.htmlTag);
            return text;
        }
    }
}