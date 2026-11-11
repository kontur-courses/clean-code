using Markdown.Inline;
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

        private bool IsEscapable(char c) => c == ItalicChar || c == EscapeChar || c == HashChar || c == LinkOpenBracket || c == LinkCloseBracket || c == UrlOpenParen || c == UrlCloseParen;

        public List<IInlineTag> ParseInlineTags(string text)
        {
            return ParseInlineTagsInternal(text, allowStrong: true);
        }

        private List<IInlineTag> ParseInlineTagsInternal(string text, bool allowStrong)
        {
            var tags = new List<IInlineTag>();
            var currentText = new StringBuilder();

            var nextClosingBrackets = BuildNextUnescapedIndexes(text, LinkCloseBracket);
            var nextClosingParens = BuildNextUnescapedIndexes(text, UrlCloseParen);
            for (int i = 0; i < text.Length; i++)
            {
                // Экранирование
                if (text[i] == EscapeChar && i + 1 < text.Length && IsEscapable(text[i + 1]))
                {
                    currentText.Append(text[i + 1]);
                    i++;
                    continue;
                }
                // Ссылки
                if (text[i] == LinkOpenBracket && TryParseLink(text, ref i, nextClosingBrackets, nextClosingParens, out var linkTag))
                {
                    FlushText(tags, currentText);
                    tags.Add(linkTag);
                    continue;
                }
                // Жирный
                if (allowStrong && text[i] == StrongChar && i + StrongDelimiter.Length - 1 < text.Length && text[i + 1] == StrongChar && TryParseStrong(text, ref i, out var strongTag))
                {
                    FlushText(tags, currentText);
                    tags.Add(strongTag);
                    continue;
                }
                // Курсив
                if (text[i] == ItalicChar && TryParseItalic(text, ref i, out var italicTag))
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

        // Сохранение накопленного текста
        private void FlushText(List<IInlineTag> tags, StringBuilder sb)
        {
            if (sb.Length > 0)
            {
                tags.Add(new TextTag(sb.ToString()));
                sb.Clear();
            }
        }

        private int FindClosingIndex(string text, string closingStr, int startIndex, bool skipNested, bool requireNonWhitespaceBefore = false)
        {
            int index = startIndex;

            while (index <= text.Length - closingStr.Length)
            {
                index = text.IndexOf(closingStr, index, System.StringComparison.Ordinal);

                if (index == -1)
                    return -1;

                if (IsEscaped(text, index))
                {
                    index += closingStr.Length;
                    continue;
                }

                if (skipNested)
                {
                    if (closingStr == ItalicDelimiter && index + StrongDelimiter.Length <= text.Length && text.AsSpan(index).StartsWith(StrongDelimiter))
                    {
                        index += StrongDelimiter.Length;
                        continue;
                    }

                    if (closingStr == StrongDelimiter && index > 0 && text[index - ItalicDelimiter.Length] == ItalicChar)
                    {
                        index += StrongDelimiter.Length;
                        continue;
                    }
                }

                if (requireNonWhitespaceBefore && (index == 0 || char.IsWhiteSpace(text[index - 1])))
                {
                    index += closingStr.Length;
                    continue;
                }

                return index;
            }
            return -1;
        }

        private bool IsEscaped(string text, int index)
        {
            int escapeCount = 0;

            for (int i = index - 1; i >= 0 && text[i] == EscapeChar; i--)
                escapeCount++;

            return escapeCount % 2 != 0;
        }

        private bool IsValidUnderscoreContext(string text, int openIndex, int closeIndex, int markerLength)
        {
            bool isDigitAfterOpen = openIndex + markerLength < text.Length && char.IsDigit(text[openIndex + markerLength]);
            bool isDigitBeforeOpen = openIndex > 0 && char.IsDigit(text[openIndex - 1]);

            bool isDigitBeforeClose = closeIndex > 0 && char.IsDigit(text[closeIndex - 1]);
            bool isDigitAfterClose = closeIndex + markerLength < text.Length && char.IsDigit(text[closeIndex + markerLength]);

            if (isDigitAfterOpen || isDigitBeforeOpen || isDigitBeforeClose || isDigitAfterClose)
                return false;

            return true;
        }

        private bool TryParseStrong(string text, ref int i, out StrongTag strongTag)
        {
            strongTag = null!;

            int contentStart = i + StrongDelimiter.Length;

            int closeIndex = FindClosingIndex(text, StrongDelimiter, contentStart, skipNested: true, requireNonWhitespaceBefore: true);

            if (!IsValidDelimiterContent(text, i, closeIndex, StrongDelimiter))
                return false;

            // Проверка цифр около разделителей.
            if (!IsValidUnderscoreContext(text, i, closeIndex, StrongDelimiter.Length))
                return false;

            bool startsInsideWord = i > 0 && char.IsLetterOrDigit(text[i - 1]);

            bool endsInsideWord = closeIndex + StrongDelimiter.Length < text.Length && char.IsLetterOrDigit(text[closeIndex + StrongDelimiter.Length]);

            if (startsInsideWord || endsInsideWord)
            {
                for (int j = contentStart; j < closeIndex; j++)
                {
                    if (char.IsWhiteSpace(text[j]))
                        return false;
                }
            }

            // Обработка слипшихся тегов (__курсив___).
            bool isTriple = closeIndex + StrongDelimiter.Length < text.Length && text[closeIndex + StrongDelimiter.Length] == ItalicChar;

            int actualEnd = isTriple ? closeIndex + ItalicDelimiter.Length : closeIndex;

            if (HasUnpairedItalicMarker(text, contentStart, actualEnd))
                return false;

            string contentStr = text.Substring(contentStart, actualEnd - contentStart);

            var innerTags = ParseInlineTagsInternal(contentStr, allowStrong: true);

            strongTag = new StrongTag(innerTags);
            i = actualEnd + StrongDelimiter.Length - 1;
            return true;
        }

        private bool TryParseItalic(string text, ref int i, out ItalicTag italicTag)
        {
            italicTag = null!;

            if (i + ItalicDelimiter.Length < text.Length && text[i + ItalicDelimiter.Length] == StrongChar)
                return false;

            if (i >= ItalicDelimiter.Length && text[i - ItalicDelimiter.Length] == StrongChar)
                return false;

            int contentStart = i + ItalicDelimiter.Length;

            int closeIndex = FindClosingIndex(text,ItalicDelimiter, contentStart, skipNested: true, requireNonWhitespaceBefore: true);

            if (!IsValidDelimiterContent(text, i, closeIndex, ItalicDelimiter))
                return false;

            if (!IsValidUnderscoreContext(text, i, closeIndex, ItalicDelimiter.Length))
                return false;

            bool startsInsideWord = i > 0 && char.IsLetterOrDigit(text[i - 1]);

            bool endsInsideWord = closeIndex + ItalicDelimiter.Length < text.Length && char.IsLetterOrDigit(text[closeIndex + ItalicDelimiter.Length]);

            if (startsInsideWord || endsInsideWord)
            {
                for (int j = contentStart; j < closeIndex; j++)
                {
                    if (char.IsWhiteSpace(text[j]))
                        return false;
                }
            }

            if (HasIntersectingStrong(text, i, closeIndex))
                return false;

            string contentStr = text.Substring(contentStart, closeIndex - contentStart);

            var innerTags = ParseInlineTagsInternal(contentStr, allowStrong: false);

            italicTag = new ItalicTag(innerTags);
            i = closeIndex;

            return true;
        }

        // Парсинг ссылки ([текст](url))
        private bool TryParseLink(string text, ref int i, int[] nextClosingBrackets, int[] nextClosingParens, out LinkTag linkTag)
        {
            linkTag = null!;

            int closeBracket = nextClosingBrackets[i + 1];

            if (closeBracket == -1 || closeBracket + 1 >= text.Length || text[closeBracket + 1] != UrlOpenParen)
                return false;

            int closeParen = nextClosingParens[closeBracket + 2];

            if (closeParen == -1)
                return false;
            string linkText = text.Substring(i + 1, closeBracket - (i + 1));
            string url = UnescapeUrl(text.Substring(closeBracket + 2, closeParen - closeBracket - 2));

            linkTag = new LinkTag(ParseInlineTags(linkText), url);
            i = closeParen;
            return true;
        }

        private bool HasIntersectingStrong(string text, int italicOpen, int italicClose)
        {
            int strongMarkersBefore = CountStrongMarkers(text, 0, italicOpen);
            if (strongMarkersBefore % 2 != 0)
            {
                for (int j = italicOpen + ItalicDelimiter.Length; j + StrongDelimiter.Length <= italicClose; j++)
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

            for (int j = italicOpen + ItalicDelimiter.Length; j + StrongDelimiter.Length <= italicClose; j++)
            {
                if (text[j] == EscapeChar)
                {
                    j++;
                    continue;
                }

                if (!text.AsSpan(j).StartsWith(StrongDelimiter))
                    continue;
                int strongClose = FindClosingIndex(text, StrongDelimiter, j + StrongDelimiter.Length, skipNested: false, requireNonWhitespaceBefore: true);

                if (strongClose == -1)
                    continue;

                if (strongClose >= italicClose)
                    return true;

                j = strongClose + StrongDelimiter.Length - 1;
            }
            return false;
        }

        private bool HasUnpairedItalicMarker(string text, int start, int end)
        {
            int italicMarkersCount = 0;

            for (int j = start; j < end;)
            {
                if (text[j] == EscapeChar && j + 1 < end && IsEscapable(text[j + 1]))
                {
                    j += 2;
                    continue;
                }

                if (text[j] != ItalicChar)
                {
                    j++;
                    continue;
                }

                int markerCount = 0;

                while (j < end && text[j] == ItalicChar)
                {
                    markerCount++;
                    j++;
                }

                if (markerCount % 2 != 0)
                    italicMarkersCount++;
            }
            return italicMarkersCount % 2 != 0;
        }

        private int CountStrongMarkers(string text, int start, int end)
        {
            int count = 0;

            for (int j = start; j + 1 < end; j++)
            {
                if (text[j] == EscapeChar)
                {
                    j++;
                    continue;
                }

                if (text[j] == StrongChar && text[j + 1] == StrongChar)
                {
                    count++;
                    j++;
                }
            }
            return count;
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
                return false;

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
    }
}