using Markdown.Inline;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Parsers
{
    public class InlineParser
    {
        private bool IsSpecialChar(char c) => c == '_' || c == '\\' || c == '#' || c == '[' || c == ']';

        public List<IInlineTag> ParseInlineTags(string text, bool allowStrong = true)
        {
            var tags = new List<IInlineTag>();
            var currentText = new StringBuilder();

            // Посимвольный обход строки
            for (int i = 0; i < text.Length; i++)
            {
                // Экранирование
                if (text[i] == '\\' && i + 1 < text.Length && IsSpecialChar(text[i + 1]))
                {
                    currentText.Append(text[i + 1]);
                    i++;
                    continue;
                }
                // Ссылки
                if (text[i] == '[' && TryParseLink(text, ref i, out var linkTag))
                {
                    FlushText(tags, currentText);
                    tags.Add(linkTag);
                    continue;
                }
                // Жирный
                if (allowStrong && text[i] == '_' && i + 1 < text.Length && text[i + 1] == '_' && TryParseStrong(text, ref i, out var strongTag))
                {
                    FlushText(tags, currentText);
                    tags.Add(strongTag);
                    continue;
                }
                // Курсив
                if (text[i] == '_' && TryParseItalic(text, ref i, out var italicTag))
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

        private bool TryParseStrong(string text, ref int i, out StrongTag strongTag)
        {
            strongTag = null!;
            int closeIndex = text.IndexOf("__", i + 2);

            // Проверка на пустоту и пробелы у границ
            if (closeIndex == -1 || closeIndex == i + 2) return false;
            if (char.IsWhiteSpace(text[i + 2]) || char.IsWhiteSpace(text[closeIndex - 1])) return false;

            // Обработка слипшихся тегов (__курсив___)
            bool isTriple = closeIndex + 2 < text.Length && text[closeIndex + 2] == '_';
            int actualEnd = isTriple ? closeIndex + 1 : closeIndex;

            var contentStr = text.Substring(i + 2, actualEnd - (i + 2));

            // Рекурсивный парсинг внутренностей (разрешаем курсив)
            var innerTags = ParseInlineTags(contentStr, allowStrong: true);

            strongTag = new StrongTag(innerTags);
            i = actualEnd + 1;
            return true;
        }

        private bool TryParseItalic(string text, ref int i, out ItalicTag italicTag)
        {
            italicTag = null!;
            // Часть жирного
            if (i + 1 < text.Length && text[i + 1] == '_') return false;
            int closeIndex = -1;

            // Ищем закрывающее _, пропуская двойные (__)
            for (int j = i + 1; j < text.Length; j++)
            {
                if (text[j] == '_')
                {
                    if (j + 1 < text.Length && text[j + 1] == '_')
                    {
                        j++;
                        continue;
                    }
                    closeIndex = j;
                    break;
                }
            }

            // Проверка на пустоту, пробелы и цифры у границ
            if (closeIndex == -1 || closeIndex == i + 1) return false;
            if (char.IsWhiteSpace(text[i + 1]) || char.IsWhiteSpace(text[closeIndex - 1])) return false;
            if (char.IsDigit(text[i + 1]) && closeIndex + 1 < text.Length && char.IsDigit(text[closeIndex + 1])) return false;

            var contentStr = text.Substring(i + 1, closeIndex - (i + 1));

            // Внутри курсива запрещаем жирный
            var innerTags = ParseInlineTags(contentStr, allowStrong: false);

            italicTag = new ItalicTag(innerTags);
            i = closeIndex;
            return true;
        }

        // Парсинг ссылки ([текст](url))
        private bool TryParseLink(string text, ref int i, out LinkTag linkTag)
        {
            linkTag = null!;
            int closeBracket = text.IndexOf(']', i + 1);

            if (closeBracket == -1 || closeBracket + 1 >= text.Length || text[closeBracket + 1] != '(') return false;
            int closeParen = text.IndexOf(')', closeBracket + 2);

            if (closeParen == -1) return false;
            string linkText = text.Substring(i + 1, closeBracket - (i + 1));
            string url = text.Substring(closeBracket + 2, closeParen - (closeBracket + 2));

            linkTag = new LinkTag(ParseInlineTags(linkText), url);
            i = closeParen;
            return true;
        }

    }
}
