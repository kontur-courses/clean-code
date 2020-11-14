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

            for (var i = 0; i < resultText.Length; ++i)
            {
                if (!Lexer.IsToken(resultText[i].ToString()))
                    continue;
                var token = Lexer.Analyze(resultText, i);
                if (token.Text.StartsWith(TokenType.Slash))
                {
                    resultText = resultText.Remove(token.Position, token.Text.Length > 1 ? 1 : 0);
                    continue;
                }
                i += token.Text.Length - 1;

                var tag = new MarkdownTag(token, 
                    !lastTagOfEachType.ContainsKey(token.Text) || !lastTagOfEachType[token.Text].IsOpened);
                if (!tag.IsValidTag(resultText))
                    continue;
                tags.Push(tag);
                lastTagOfEachType[tag.Value] = tag;
                resultText = HandlePairedTags(resultText, tags, ref i);
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
            ref int index)
        {
            var oldLength = text.Length;
            if (CanReplacePairedTags(text.ToString(), tags))
                text = text.ReplaceMarkdownTagsOnHtmlTags(tags.Pop(), tags.Pop());
            index += text.Length - oldLength;
            return text;
        }

        private static bool CanReplacePairedTags(string text, Stack<MarkdownTag> tags)
        {
            if (tags.Count < 2)
                return false;

            var lastTag = tags.Pop();
            var preLastTag = tags.Pop();
            var correctSequenceBoldAndItalic = IsBoldAndItalicInCorrectSequence(lastTag, tags);

            tags.Push(preLastTag);
            tags.Push(lastTag);
            return lastTag.Value == preLastTag.Value
                   && preLastTag.IsOpened && !lastTag.IsOpened
                   && !IsEmptyBetweenTags(preLastTag, lastTag)
                   && correctSequenceBoldAndItalic
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

        private static bool IsBoldAndItalicInCorrectSequence(MarkdownTag lastTag, Stack<MarkdownTag> tags)
            => lastTag.Value != TokenType.Bold || tags.Count == 0 || tags.Peek().Value != TokenType.Italic;
        
        private static bool IsEmptyBetweenTags(MarkdownTag preLastTag, MarkdownTag lastTag)
            => lastTag.StartPosition - preLastTag.EndPosition == 1;
    }
}