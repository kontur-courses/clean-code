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
                DeleteUnnecessarySlashFromTags(i, tags);
                if (resultText[i] == '\\')
                    resultText = TryHandleSlash(tags, resultText, ref i);

                if (!MarkdownTag.IsTag(resultText[i].ToString()))
                    continue;
                var tag = GetCorrectTag(resultText, ref i, lastTagOfEachStyles);
                if (tag.IsShieldedTag(tags))
                {
                    resultText = resultText.Remove(tags.Peek().StartPosition, 1);
                    i--;
                    continue;
                }
                if (!tag.IsValidTag(resultText))
                    continue;

                tags.Push(tag);
                lastTagOfEachStyles[tag.Value] = tag;
                resultText = TryReplaceMarkdownTagsOnHtmlTags(resultText, tags, ref i);
            }

            return resultText.ToString();
        }

        private static StringBuilder TryHandleSlash(Stack<MarkdownTag> tags, StringBuilder text, ref int index)
        {
            tags.Push(new MarkdownTag(text[index].ToString(), index, true));
            if (!CanShieldSlash(tags)) 
                return text;
            index--;
            return text.ShieldSlash(tags.Pop(), tags.Pop());
        }

        private static StringBuilder TryReplaceMarkdownTagsOnHtmlTags(StringBuilder text, Stack<MarkdownTag> tags,
            ref int index)
        {
            var oldLength = text.Length;
            if (CanReplaceMarkdownTagsOnHtmlTags(text.ToString(), tags))
                text = text.ReplaceMarkdownTagsOnHtmlTags(tags.Pop(), tags.Pop());
            index += text.Length - oldLength;
            return text;
        }

        private static void DeleteUnnecessarySlashFromTags(int index, Stack<MarkdownTag> tags)
        {
            if (tags.Count == 0)
                return;
            if (tags.Peek().Value == @"\" && index - tags.Peek().EndPosition > 1)
                tags.Pop();
        }

        private static MarkdownTag GetCorrectTag(StringBuilder text, ref int index, Dictionary<string, MarkdownTag> lastTags)
        {
            var value = text[index].ToString();
            var tag = new MarkdownTag(value, index, !lastTags.ContainsKey(value) || !lastTags[value].IsOpened);
            if (!tag.IsBold(text))
                return tag;
            index++;
            value = tag.Value + tag.Value;
            return new MarkdownTag(value, tag.StartPosition, !lastTags.ContainsKey(value) || !lastTags[value].IsOpened);
        }

        private static bool CanReplaceMarkdownTagsOnHtmlTags(string text, Stack<MarkdownTag> tags)
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
                   && IsValidStringBetweenBoldOrItalic(text, preLastTag, lastTag);
        }

        private static bool CanShieldSlash(Stack<MarkdownTag> tags)
        {
            if (tags.Count < 2)
                return false;

            var lastSymbol = tags.Pop();
            var preLastSymbol = tags.Peek();
            tags.Push(lastSymbol);

            return lastSymbol.Value == preLastSymbol.Value;
        }

        private static bool IsValidStringBetweenBoldOrItalic(string text, MarkdownTag preLastTag, MarkdownTag lastTag)
        {
            if (lastTag.Value != Styles.Italic && lastTag.Value != Styles.Bold)
                return true;

            var isSpaceBetweenTags = text
                .Substring(preLastTag.EndPosition + 1, lastTag.StartPosition - preLastTag.EndPosition - 1)
                .Contains(" ");
            var isSpaceBeforeOpenedTag =
                preLastTag.StartPosition - 1 < 0 || char.IsWhiteSpace(text[preLastTag.StartPosition - 1]);
            var isSpaceAfterClosedTag =
                lastTag.EndPosition + 1 >= text.Length || char.IsWhiteSpace(text[lastTag.EndPosition + 1]);

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