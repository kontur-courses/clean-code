using System;
using System.Collections.Generic;
using System.Linq;
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
            var ignoreIndex = -1;

            foreach (var token in Lexer.Analyze(text))
            {
                if (token.Position <= ignoreIndex)
                    continue;
                if (token.Text.StartsWith(TokenType.Slash))
                {
                    resultText = resultText.Remove(token.Position + shift, 1);
                    shift--;
                    continue;
                }
                if (token.Text.StartsWith(TokenType.SquareBracket))
                {
                    var link = GetPartsOfLink(text, token.Position);
                    if (link == null)
                        continue;
                    var resultToken = new Token(string.Join("", link), token.Position);
                    resultText = resultText.HandleLink(resultToken, link[4], link[1], ref shift);
                    ignoreIndex = resultToken.Position + resultToken.Text.Length - 1;
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

        private static List<string> GetPartsOfLink(string text, int start)
        {
            var resultList = new List<string>();
            var tokenValue = new StringBuilder();
            var position = 0;
            var j = start;
            for (; j < text.Length; ++j)
            {
                var symbol = text[j].ToString();
                if (Lexer.PartsOfLink.Where(x => x != Lexer.PartsOfLink[position]).Contains(symbol))
                    return null;
                if (symbol != Lexer.PartsOfLink[position])
                {
                    tokenValue.Append(symbol);
                    continue;
                }
                if (j - 1 >= 0 && text[j - 1].ToString() == TokenType.Slash)
                    return null;
                if (tokenValue.Length > 0)
                    resultList.Add(tokenValue.ToString());
                resultList.Add(symbol);

                tokenValue.Clear();
                if (++position == Lexer.PartsOfLink.Count)
                    break;
            }
            return IsCorrectLink(resultList, text.Substring(start + 1, j - start)) ? resultList : null;
        }

        private static bool IsCorrectLink(List<string> resultList, string linkText)
        {
            var roundBracketPosition = linkText.IndexOf(TokenType.RoundBracket, StringComparison.Ordinal);
            return linkText[roundBracketPosition - 1].ToString() == TokenType.BackSquareBracket
                   && !resultList[4].Contains(" ");
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