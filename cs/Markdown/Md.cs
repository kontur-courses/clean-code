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
            var lastTagOfEachType = new Dictionary<string, MarkdownTag>();
            var resultText = new StringBuilder(text);
            var shift = 0;

            foreach (var token in Lexer.Analyze(text))
            {
                if (token.Text.StartsWith(TokenType.Slash))
                {
                    resultText = resultText.Remove(token.Position + shift, 1);
                    shift--;
                    continue;
                }

                var tag = new MarkdownTag(token.Text, token.Position + shift,
                    !lastTagOfEachType.ContainsKey(token.Text) || !lastTagOfEachType[token.Text].IsOpened);
                if (!tag.IsValidTag(resultText))
                    continue;

                tags.Push(tag);
                lastTagOfEachType[tag.Value] = tag;
                if (tags.Count >= 2)
                    resultText = HandlePairedTags(resultText, tags, lastTagOfEachType, ref shift);
            }
            return TryHandleHeading(lastTagOfEachType, resultText);
        }

        private static string TryHandleHeading(Dictionary<string, MarkdownTag> lastTagOfEachType,
            StringBuilder resultText)
        {
            return lastTagOfEachType.ContainsKey(TokenType.Heading)
                ? resultText.ReplaceMarkdownTagsOnHtmlTags(lastTagOfEachType[TokenType.Heading]).ToString()
                : resultText.ToString();
        }

        private static StringBuilder HandlePairedTags(StringBuilder text, Stack<MarkdownTag> tags,
            Dictionary<string, MarkdownTag> lastTagOfEachType, ref int index)
        {
            var lastTag = tags.Pop();
            var preLastTag = tags.Pop();

            if (lastTag.Value != preLastTag.Value)
            {
                tags.Push(preLastTag);
                tags.Push(lastTag);
                return text;
            }

            if (!CanReplacePairedTags(text.ToString(), lastTag, preLastTag))
                return text;
            text = text.ReplaceMarkdownTagsOnHtmlTags(lastTag, preLastTag, ref index);

            return !IsBoldAndItalicInCorrectSequence(lastTagOfEachType, lastTag, preLastTag)
                ? text.ReplaceHtmlTagsOnMarkdownTags(lastTagOfEachType[TokenType.Bold],
                    preLastTag.EndPosition + 1, lastTag.StartPosition - 1, ref index)
                : text;
        }

        private static bool CanReplacePairedTags(string text, MarkdownTag lastTag, MarkdownTag preLastTag)
        {
            return preLastTag.IsOpened
                   && !lastTag.IsOpened
                   && !IsEmptyBetweenTags(preLastTag, lastTag)
                   && IsValidStringBetweenBoldOrItalic(text, preLastTag, lastTag);
        }

        private static bool IsValidStringBetweenBoldOrItalic(string text, MarkdownTag preLastTag, MarkdownTag lastTag)
        {
            if (lastTag.Value != TokenType.Italic && lastTag.Value != TokenType.Bold)
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

        private static bool IsBoldAndItalicInCorrectSequence(Dictionary<string, MarkdownTag> lastTagOfEachType,
            MarkdownTag lastTag, MarkdownTag preLastTag)
        {
            if (!lastTagOfEachType.ContainsKey(TokenType.Bold) || lastTag.Value != TokenType.Italic)
                return true;
            return !(lastTagOfEachType[TokenType.Bold].StartPosition > preLastTag.EndPosition
                     && lastTagOfEachType[TokenType.Bold].EndPosition < lastTag.StartPosition);
        }

        private static bool IsEmptyBetweenTags(MarkdownTag preLastTag, MarkdownTag lastTag) 
            => lastTag.StartPosition - preLastTag.EndPosition == 1;
    }
}