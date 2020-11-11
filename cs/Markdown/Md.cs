using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class Md
    {
        public static string Render(string text)
        {
            if (text == null)
                throw new NullReferenceException("input must be null");
            var tags = new Stack<MarkdownTag>();
            var lastTagOfEachStyles = new Dictionary<string, MarkdownTag>();
            var resultText = new StringBuilder(text);

            for (var i = 0; i < resultText.Length; ++i)
            {
                if (!MarkdownTag.IsTag(resultText[i].ToString()))
                    continue;

                var tag = GetTag(resultText[i].ToString(), i, lastTagOfEachStyles);
                if (tag.IsBold(resultText))
                {
                    tag = GetTag(new string(resultText[i], 2), i, lastTagOfEachStyles);
                    i++;
                }

                if (!tag.IsValidTag(resultText))
                    continue;

                tags.Push(tag);
                lastTagOfEachStyles[tag.Value] = tag;

                var oldLength = resultText.Length;
                if (CanReplaceMarkdownTagsOnHtmlTags(resultText.ToString(), tags))
                    resultText = resultText.ReplaceMarkdownTagsOnHtmlTags(tags.Pop(), tags.Pop());
                i += resultText.Length - oldLength;
            }

            return resultText.ToString();
        }

        private static MarkdownTag GetTag(string value, int index, Dictionary<string, MarkdownTag> lastTags)
        {
            return new MarkdownTag(value, index, !lastTags.ContainsKey(value) || !lastTags[value].IsOpened);
        }

        private static bool CanReplaceMarkdownTagsOnHtmlTags(string resultText, Stack<MarkdownTag> tags)
        {
            if (tags.Count < 2)
                return false;

            var lastTag = tags.Pop();
            var preLastTag = tags.Pop();
            var correctSequence = IsBoldAndItalicInCorrectSequence(lastTag, tags);

            tags.Push(preLastTag);
            tags.Push(lastTag);
            return lastTag.Value == preLastTag.Value
                   && preLastTag.IsOpened && !lastTag.IsOpened
                   && !IsEmptyBetweenTags(preLastTag, lastTag) && correctSequence
                   && IsValidStringBetweenBoldOrItalic(resultText, preLastTag, lastTag);
        }

        private static bool IsValidStringBetweenBoldOrItalic(string resultText,
            MarkdownTag preLastTag, MarkdownTag lastTag)
        {
            if (lastTag.Value != Styles.Italic && lastTag.Value != Styles.Bold)
                return true;

            var isSpaceBetweenTags = resultText
                .Substring(preLastTag.EndPosition + 1, lastTag.StartPosition - preLastTag.EndPosition - 1)
                .Contains(" ");
            var isSpaceBeforeOpenedTag =
                preLastTag.StartPosition - 1 < 0 || char.IsWhiteSpace(resultText[preLastTag.StartPosition - 1]);
            var isSpaceAfterClosedTag =
                lastTag.EndPosition + 1 >= resultText.Length || char.IsWhiteSpace(resultText[lastTag.EndPosition + 1]);

            return !isSpaceBetweenTags || isSpaceBeforeOpenedTag && isSpaceAfterClosedTag;
        }

        private static bool IsBoldAndItalicInCorrectSequence(MarkdownTag lastTag, Stack<MarkdownTag> tags)
        {
            return lastTag.Value != Styles.Bold || tags.Count == 0 || tags.Peek().Value != Styles.Italic;
        }

        private static bool IsEmptyBetweenTags(MarkdownTag preLastTag, MarkdownTag lastTag)
        {
            return lastTag.StartPosition - preLastTag.EndPosition == 1;
        }
    }
}