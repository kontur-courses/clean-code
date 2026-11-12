using Markdown.Inline;
using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Parsers
{
    public class InlineParser
    {
        private const char EscapeChar = '\\';
        private const char ItalicChar = '_';
        private const char LinkOpenBracket = '[';
        private const char LinkCloseBracket = ']';
        private const char UrlOpenParen = '(';
        private const char UrlCloseParen = ')';
        private const char StrongChar = '_';
        private const char HashChar = '#';
        private const string ItalicDelimiter = "_";
        private const string StrongDelimiter = "__";

        private bool IsEscapable(char c) => c == ItalicChar || c == EscapeChar || c == HashChar || c == LinkOpenBracket ||
            c == LinkCloseBracket || c == UrlOpenParen || c == UrlCloseParen;

        public List<IInlineTag> ParseInlineTags(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            return ParseInlineTagsInternal(text, allowStrong: true, allowLinks: true);
        }

        private sealed class DelimiterPrefixIndex
        {
            private readonly bool[] italicParity;
            private readonly bool[] strongParity;
            private readonly int[] nextItalicClosing;
            private readonly int[] nextStrongClosing;
            private readonly int[] nextStrongClosingWithoutSkip;

            public DelimiterPrefixIndex(string text)
            {
                int length = text.Length;

                italicParity = new bool[length + 1];
                strongParity = new bool[length + 1];

                nextItalicClosing = new int[length + 1];
                nextStrongClosing = new int[length + 1];
                nextStrongClosingWithoutSkip = new int[length + 1];

                BuildItalicParity(text);
                BuildStrongParity(text);
                BuildClosingIndexes(text);
            }

            public bool HasUnmatchedItalicBefore(int end)
            {
                return italicParity[end];
            }

            public bool HasOddStrongMarkersBefore(int end)
            {
                return strongParity[end];
            }

            public int FindClosingIndex(string delimiter, int startIndex, bool skipNested, bool requireNonWhitespaceBefore)
            {
                if (startIndex < 0)
                    startIndex = 0;

                if (delimiter == ItalicDelimiter && skipNested && requireNonWhitespaceBefore)
                {
                    return nextItalicClosing[startIndex];
                }

                if (delimiter == StrongDelimiter && requireNonWhitespaceBefore)
                {
                    return skipNested ? nextStrongClosing[startIndex] : nextStrongClosingWithoutSkip[startIndex];
                }
                throw new NotSupportedException(
                    "Unsupported delimiter search configuration");
            }

            private void BuildItalicParity(string text)
            {
                bool parity = false;

                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == EscapeChar)
                    {
                        italicParity[i + 1] = parity;
                        if (i + 1 < text.Length)
                        {
                            i++;
                            italicParity[i + 1] = parity;
                        }
                        continue;
                    }

                    if (text[i] == ItalicChar)
                        parity = !parity;

                    italicParity[i + 1] = parity;
                }
            }

            private void BuildStrongParity(string text)
            {
                var toggles = new bool[text.Length + 1];
                for (int i = 0; i + 1 < text.Length;)
                {
                    if (text[i] == EscapeChar)
                    {
                        i += 2;
                        continue;
                    }

                    if (text[i] == StrongChar &&
                        text[i + 1] == StrongChar)
                    {
                        int firstEnd = i + 2;

                        toggles[firstEnd] = !toggles[firstEnd];

                        if (firstEnd < text.Length &&
                            char.IsWhiteSpace(text[firstEnd]))
                        {
                            int removeAt = firstEnd + 1;
                            toggles[removeAt] = !toggles[removeAt];
                        }
                        i += 2;
                        continue;
                    }
                    i++;
                }

                bool parity = false;

                for (int i = 0; i <= text.Length; i++)
                {
                    parity ^= toggles[i];
                    strongParity[i] = parity;
                }
            }

            private void BuildClosingIndexes(string text)
            {
                int length = text.Length;
                var escaped = new bool[length];
                int escapeCount = 0;

                for (int i = 0; i < length; i++)
                {
                    if (text[i] == EscapeChar)
                    {
                        escapeCount++;
                        continue;
                    }

                    escaped[i] = escapeCount % 2 != 0;
                    escapeCount = 0;
                }

                nextItalicClosing[length] = -1;
                nextStrongClosing[length] = -1;
                nextStrongClosingWithoutSkip[length] = -1;

                for (int i = length - 1; i >= 0; i--)
                {
                    if (text[i] != ItalicChar)
                    {
                        nextItalicClosing[i] = nextItalicClosing[i + 1];
                    }
                    else if (escaped[i])
                    {
                        nextItalicClosing[i] = nextItalicClosing[i + 1];
                    }
                    else if (i + 1 < length && text[i + 1] == ItalicChar)
                    {
                        nextItalicClosing[i] = nextItalicClosing[Math.Min(i + 2, length)];
                    }
                    else if (i == 0 || char.IsWhiteSpace(text[i - 1]))
                    {
                        nextItalicClosing[i] = nextItalicClosing[i + 1];
                    }
                    else
                    {
                        nextItalicClosing[i] = i;
                    }
                    bool isStrongMarker = i + 1 < length && text[i] == StrongChar && text[i + 1] == StrongChar;

                    if (!isStrongMarker)
                    {
                        nextStrongClosingWithoutSkip[i] = nextStrongClosingWithoutSkip[i + 1];
                    }
                    else if (escaped[i] || i == 0 || char.IsWhiteSpace(text[i - 1]))
                    {
                        nextStrongClosingWithoutSkip[i] = nextStrongClosingWithoutSkip[Math.Min(i + 2, length)];
                    }
                    else
                    {
                        nextStrongClosingWithoutSkip[i] = i;
                    }

                    if (!isStrongMarker)
                    {
                        nextStrongClosing[i] = nextStrongClosing[i + 1];
                    }
                    else if (escaped[i] || i == 0 || text[i - 1] == ItalicChar || char.IsWhiteSpace(text[i - 1]))
                    {
                        nextStrongClosing[i] = nextStrongClosing[Math.Min(i + 2, length)];
                    }
                    else
                    {
                        nextStrongClosing[i] = i;
                    }
                }
            }
        }

        private List<IInlineTag> ParseInlineTagsInternal(string text, bool allowStrong, bool allowLinks)
        {
            var tags = new List<IInlineTag>();
            var currentText = new StringBuilder();
            var delimiterIndex = new DelimiterPrefixIndex(text);
            int[] nextClosingBrackets = Array.Empty<int>();
            int[] nextClosingParens = Array.Empty<int>();

            if (allowLinks)
            {
                nextClosingBrackets = BuildNextUnescapedIndexes(text, LinkCloseBracket);

                nextClosingParens = BuildNextUnescapedIndexes(text, UrlCloseParen);
            }

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == EscapeChar && i + 1 < text.Length && IsEscapable(text[i + 1]))
                {
                    currentText.Append(text[i + 1]);
                    i++;
                    continue;
                }
                // Ссылки
                if (allowLinks && text[i] == LinkOpenBracket && TryParseLink(text, ref i, nextClosingBrackets, nextClosingParens,
                        out var linkTag))
                {
                    FlushText(tags, currentText);
                    tags.Add(linkTag);
                    continue;
                }
                // Жирный
                if (allowStrong && text[i] == StrongChar && i + 1 < text.Length && text[i + 1] == StrongChar &&
                    TryParseStrong(text, ref i, allowLinks, delimiterIndex, out var strongTag))
                {
                    FlushText(tags, currentText);
                    tags.Add(strongTag);
                    continue;
                }
                // Курсив
                if (text[i] == ItalicChar && TryParseItalic(text, ref i, allowLinks, delimiterIndex, out var italicTag))
                {
                    FlushText(tags, currentText);
                    tags.Add(italicTag);
                    continue;
                }
                currentText.Append(text[i]);
            }
            FlushText(tags, currentText);
            return tags;
        }

        private void FlushText(List<IInlineTag> tags, StringBuilder sb)
        {
            if (sb.Length > 0)
            {
                tags.Add(new TextTag(sb.ToString()));
                sb.Clear();
            }
        }

        private bool IsEscaped(string text, int index)
        {
            int escapeCount = 0;
            for (int i = index - 1; i >= 0 && text[i] == EscapeChar; i--)
            {
                escapeCount++;
            }
            return escapeCount % 2 != 0;
        }

        private bool IsValidUnderscoreContext(string text, int openIndex, int closeIndex, int markerLength)
        {
            bool isDigitAfterOpen = openIndex + markerLength < text.Length && char.IsDigit(text[openIndex + markerLength]);
            bool isDigitBeforeOpen = openIndex > 0 && char.IsDigit(text[openIndex - 1]);
            bool isDigitBeforeClose = closeIndex > 0 && char.IsDigit(text[closeIndex - 1]);
            bool isDigitAfterClose = closeIndex + markerLength < text.Length && char.IsDigit(text[closeIndex + markerLength]);
            return !(isDigitAfterOpen || isDigitBeforeOpen || isDigitBeforeClose || isDigitAfterClose);
        }

        private bool TryParseStrong(string text, ref int i, bool allowLinks, DelimiterPrefixIndex delimiterIndex,
            out StrongTag strongTag)
        {
            strongTag = null!;
            int contentStart = i + StrongDelimiter.Length;
            int closeIndex = delimiterIndex.FindClosingIndex(StrongDelimiter, contentStart, skipNested: true,
                requireNonWhitespaceBefore: true);

            if (!IsValidDelimiterContent(text, i, closeIndex, StrongDelimiter))
            {
                return false;
            }

            if (!IsValidUnderscoreContext(text, i, closeIndex, StrongDelimiter.Length))
            {
                return false;
            }

            bool startsInsideWord = i > 0 && char.IsLetterOrDigit(text[i - 1]);
            bool endsInsideWord = closeIndex + StrongDelimiter.Length < text.Length && 
                char.IsLetterOrDigit(text[closeIndex + StrongDelimiter.Length]);

            if (startsInsideWord || endsInsideWord)
            {
                for (int j = contentStart; j < closeIndex; j++)
                {
                    if (char.IsWhiteSpace(text[j]))
                        return false;
                }
            }
            bool isTriple = closeIndex + StrongDelimiter.Length < text.Length &&
                text[closeIndex + StrongDelimiter.Length] == ItalicChar;

            int actualEnd = isTriple ? closeIndex + ItalicDelimiter.Length : closeIndex;
            if (HasCrossingItalic(text, contentStart, closeIndex, delimiterIndex))
            {
                return false;
            }
            string contentStr = text.Substring(contentStart, actualEnd - contentStart);
            var innerTags = ParseInlineTagsInternal(contentStr, allowStrong: true, allowLinks: allowLinks);
            strongTag = new StrongTag(innerTags);
            i = actualEnd + StrongDelimiter.Length - 1;
            return true;
        }

        private bool TryParseItalic(string text, ref int i, bool allowLinks, DelimiterPrefixIndex delimiterIndex,
            out ItalicTag italicTag)
        {
            italicTag = null!;
            if (i + 1 < text.Length && text[i + 1] == StrongChar && !IsEscaped(text, i + 1))
            {
                return false;
            }

            if (i > 0 && text[i - 1] == StrongChar && !IsEscaped(text, i - 1))
            {
                return false;
            }
            int contentStart = i + ItalicDelimiter.Length;
            int closeIndex = delimiterIndex.FindClosingIndex(ItalicDelimiter, contentStart, skipNested: true,
                requireNonWhitespaceBefore: true);
            if (!IsValidDelimiterContent(text, i, closeIndex, ItalicDelimiter))
            {
                return false;
            }
            if (!IsValidUnderscoreContext(text, i, closeIndex, ItalicDelimiter.Length))
            {
                return false;
            }
            bool startsInsideWord = i > 0 && char.IsLetterOrDigit(text[i - 1]);
            bool endsInsideWord = closeIndex + ItalicDelimiter.Length < text.Length &&
                char.IsLetterOrDigit(text[closeIndex + ItalicDelimiter.Length]);

            if (startsInsideWord || endsInsideWord)
            {
                for (int j = contentStart; j < closeIndex; j++)
                {
                    if (char.IsWhiteSpace(text[j]))
                        return false;
                }
            }
            if (HasIntersectingStrong(text, i, closeIndex, delimiterIndex))
            {
                return false;
            }
            string contentStr = text.Substring(contentStart, closeIndex - contentStart);
            var innerTags = ParseInlineTagsInternal(contentStr, allowStrong: false, allowLinks: allowLinks);
            italicTag = new ItalicTag(innerTags);
            i = closeIndex;
            return true;
        }

        private bool TryParseLink(string text, ref int i, int[] nextClosingBrackets, int[] nextClosingParens, out LinkTag linkTag)
        {
            linkTag = null!;
            int closeBracket = nextClosingBrackets[i + 1];
            if (closeBracket == -1 || closeBracket + 1 >= text.Length || text[closeBracket + 1] != UrlOpenParen)
            {
                return false;
            }

            int closeParen = nextClosingParens[closeBracket + 2];
            if (closeParen == -1)
                return false;

            string linkText = text.Substring(i + 1, closeBracket - (i + 1));
            string url = UnescapeUrl(text.Substring(closeBracket + 2, closeParen - closeBracket - 2));
            linkTag = new LinkTag(ParseInlineTagsInternal(linkText, allowStrong: true, allowLinks: false), url);
            i = closeParen;
            return true;
        }

        private bool HasIntersectingStrong(string text, int italicOpen, int italicClose, DelimiterPrefixIndex delimiterIndex)
        {
            bool hasStrongBefore = delimiterIndex.HasOddStrongMarkersBefore(italicOpen);
            if (hasStrongBefore)
            {
                for (int j = italicOpen + 1; j + StrongDelimiter.Length <= italicClose; j++)
                {
                    if (text[j] == EscapeChar)
                    {
                        j++;
                        continue;
                    }
                    if (text.AsSpan(j).StartsWith(StrongDelimiter))
                        return true;
                }
            }
            for (int j = italicOpen + 1; j + StrongDelimiter.Length <= italicClose; j++)
            {
                if (text[j] == EscapeChar)
                {
                    j++;
                    continue;
                }
                if (!text.AsSpan(j).StartsWith(StrongDelimiter))
                    continue;
                int strongClose = delimiterIndex.FindClosingIndex(StrongDelimiter, j + StrongDelimiter.Length,
                    skipNested: false, requireNonWhitespaceBefore: true);

                if (strongClose == -1)
                    continue;
                if (strongClose >= italicClose)
                    return true;
                j = strongClose + StrongDelimiter.Length - 1;
            }
            return false;
        }

        private int[] BuildNextUnescapedIndexes(string text, char target)
        {
            var escaped = new bool[text.Length];
            int escapeCount = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == EscapeChar)
                {
                    escapeCount++;
                    continue;
                }
                escaped[i] = escapeCount % 2 != 0;
                escapeCount = 0;
            }
            var nextIndexes = new int[text.Length + 1];
            nextIndexes[text.Length] = -1;
            int nextIndex = -1;
            for (int i = text.Length - 1; i >= 0; i--)
            {
                if (text[i] == target && !escaped[i])
                    nextIndex = i;

                nextIndexes[i] = nextIndex;
            }
            return nextIndexes;
        }

        private bool IsValidDelimiterContent(string text, int openIndex, int closeIndex, string delimiter)
        {
            int contentStart = openIndex + delimiter.Length;
            if (closeIndex == -1 || closeIndex == contentStart)
                return false;
            if (char.IsWhiteSpace(text[contentStart]) || char.IsWhiteSpace(text[closeIndex - 1]))
            {
                return false;
            }
            return true;
        }

        private string UnescapeUrl(string url)
        {
            var result = new StringBuilder();
            for (int i = 0; i < url.Length; i++)
            {
                if (url[i] == EscapeChar && i + 1 < url.Length && IsEscapable(url[i + 1]))
                {
                    result.Append(url[++i]);
                }
                else
                {
                    result.Append(url[i]);
                }
            }
            return result.ToString();
        }

        private bool HasCrossingItalic(string text, int strongStart, int strongEnd, DelimiterPrefixIndex delimiterIndex)
        {
            if (delimiterIndex.HasUnmatchedItalicBefore(strongStart))
            {
                int italicClose = delimiterIndex.FindClosingIndex(ItalicDelimiter, strongStart, skipNested: true,
                    requireNonWhitespaceBefore: true);

                if (italicClose >= strongStart && italicClose < strongEnd)
                {
                    return true;
                }
            }

            for (int j = strongStart; j < strongEnd;)
            {
                if (text[j] == EscapeChar)
                {
                    j += 2;
                    continue;
                }
                if (j + 1 < text.Length && text[j] == ItalicChar && text[j + 1] == ItalicChar)
                {
                    j += 2;
                    continue;
                }
                if (text[j] != ItalicChar)
                {
                    j++;
                    continue;
                }
                int italicClose = delimiterIndex.FindClosingIndex(ItalicDelimiter, j + 1, skipNested: true,
                    requireNonWhitespaceBefore: true);

                if (italicClose == -1)
                {
                    j++;
                    continue;
                }

                if (italicClose == strongEnd + 2 && strongEnd + 2 < text.Length && text.AsSpan(strongEnd).StartsWith("___"))
                {
                    j++;
                    continue;
                }

                if (italicClose > strongEnd)
                    return true;
                j++;
            }
            return false;
        }
    }
}
