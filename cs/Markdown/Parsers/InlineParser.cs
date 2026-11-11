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

        private const int StrongLength = 2;
        private const int ItalicLength = 1;

        private bool IsEscapable(char c) => c == ItalicChar || c == EscapeChar || c == HashChar || c == LinkOpenBracket || c == LinkCloseBracket || c == UrlOpenParen || c == UrlCloseParen;

        public List<IInlineTag> ParseInlineTags(string text)
        {
            return ParseInlineTagsInternal(text, allowStrong: true);
        }

        private List<IInlineTag> ParseInlineTagsInternal(string text, bool allowStrong)
        {
            var tags = new List<IInlineTag>();
            var currentText = new StringBuilder();

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
                if (text[i] == LinkOpenBracket && TryParseLink(text, ref i, out var linkTag))
                {
                    FlushText(tags, currentText);
                    tags.Add(linkTag);
                    continue;
                }
                // Жирный
                if (allowStrong && text[i] == StrongChar && i + StrongLength - 1 < text.Length && text[i + 1] == StrongChar && TryParseStrong(text, ref i, out var strongTag))
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


        private int FindClosingIndex(string text, string closingStr, int startIndex, bool skipNested)
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
                    if (closingStr.Length == 1 &&
                        index + 1 < text.Length &&
                        text[index + 1] == StrongChar)
                    {
                        index += StrongLength;
                        continue;
                    }

                    if (closingStr.Length == StrongLength &&
                        index > 0 &&
                        text[index - 1] == ItalicChar)
                    {
                        index += StrongLength;
                        continue;
                    }
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
            string closing = new string(StrongChar, StrongLength);
            int closeIndex = FindClosingIndex(text, closing, i + StrongLength, skipNested: true);

            if (closeIndex == -1 || closeIndex == i + StrongLength) return false;
            if (char.IsWhiteSpace(text[i + StrongLength]) || char.IsWhiteSpace(text[closeIndex - 1])) return false;

            // Проверка цифр
            if (!IsValidUnderscoreContext(text, i, closeIndex, StrongLength)) return false;

            bool startsInsideWord = i > 0 && char.IsLetterOrDigit(text[i - 1]);
            bool endsInsideWord = closeIndex + StrongLength < text.Length && char.IsLetterOrDigit(text[closeIndex + StrongLength]);

            if ((startsInsideWord || endsInsideWord) && text.Substring(i + StrongLength, closeIndex - (i + StrongLength)).Contains(' ')) return false;

            // Обработка слипшихся тегов (__курсив___)
            bool isTriple = closeIndex + StrongLength < text.Length && text[closeIndex + StrongLength] == ItalicChar;

            int actualEnd = isTriple ? closeIndex + ItalicLength : closeIndex;

            if (HasUnpairedItalicMarker(text, i + StrongLength, actualEnd))
                return false;

            var contentStr = text.Substring(i + StrongLength, actualEnd - (i + StrongLength));

            // Рекурсивный парсинг внутренностей (разрешаем курсив)
            var innerTags = ParseInlineTagsInternal(contentStr, allowStrong: true);

            strongTag = new StrongTag(innerTags);
            i = actualEnd + StrongLength - 1;
            return true;
        }

        private bool TryParseItalic(string text, ref int i, out ItalicTag italicTag)
        {
            italicTag = null!;

            if (i + 1 < text.Length && text[i + 1] == StrongChar)
                return false;

            if (i > 0 && text[i - 1] == StrongChar)
                return false;

            int closeIndex = FindClosingIndex(text, ItalicChar.ToString(), i + ItalicLength, skipNested: true);

            if (closeIndex == -1 || closeIndex == i + ItalicLength)
                return false;

            if (char.IsWhiteSpace(text[i + ItalicLength]) || char.IsWhiteSpace(text[closeIndex - 1]))
                return false;

            if (!IsValidUnderscoreContext(text, i, closeIndex, ItalicLength))
                return false;

            bool startsInsideWord = i > 0 && char.IsLetterOrDigit(text[i - 1]);

            bool endsInsideWord = closeIndex + ItalicLength < text.Length && char.IsLetterOrDigit(text[closeIndex + ItalicLength]);

            if ((startsInsideWord || endsInsideWord) && text.Substring(i + ItalicLength, closeIndex - (i + ItalicLength)).Contains(' '))
                return false;

            if (HasIntersectingStrong(text, i, closeIndex))
                return false;

            var contentStr = text.Substring(i + ItalicLength, closeIndex - (i + ItalicLength));

            var innerTags = ParseInlineTagsInternal(contentStr, allowStrong: false);

            italicTag = new ItalicTag(innerTags);
            i = closeIndex;

            return true;
        }

        // Парсинг ссылки ([текст](url))
        private bool TryParseLink(string text, ref int i, out LinkTag linkTag)
        {
            linkTag = null!;
            int closeBracket = FindClosingIndex(text, LinkCloseBracket.ToString(), i + 1, skipNested: false);

            if (closeBracket == -1 || closeBracket + 1 >= text.Length || text[closeBracket + 1] != UrlOpenParen) return false;

            int closeParen = FindClosingIndex(text, UrlCloseParen.ToString(), closeBracket + 2, skipNested: false);

            if (closeParen == -1) return false;

            string linkText = text.Substring(i + 1, closeBracket - (i + 1));
            string url = text.Substring(closeBracket + 2, closeParen - (closeBracket + 2));

            linkTag = new LinkTag(ParseInlineTags(linkText), url);
            i = closeParen;
            return true;
        }

        private bool HasIntersectingStrong(string text, int italicOpen, int italicClose)
        {
            for (int j = italicOpen + 1; j < italicClose; j++)
            {
                if (text[j] != StrongChar)
                    continue;

                if (j + 1 < text.Length && text[j + 1] == StrongChar)
                {
                    int strongClose = text.IndexOf("__", j + StrongLength, System.StringComparison.Ordinal);

                    if (strongClose == -1)
                        continue;

                    if (strongClose + 1 >= italicClose)
                        return true;

                    j = strongClose + 1;
                }
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
    }
}