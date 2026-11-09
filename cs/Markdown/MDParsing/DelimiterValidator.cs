using System;

namespace Markdown.Parsing
{

    public static class DelimiterValidator
    {

        public static bool IsValid(
            string text,
            int openIndex,
            int closeIndex,
            string delimiter,
            string innerContent,
            bool isDoubleUnderscore)
        {
            // Правило 1: Пустая строка внутри не считается выделением (____)
            if (string.IsNullOrWhiteSpace(innerContent))
                return false;

            // Правило 2: После открывающего должен идти непробельный символ
            if (openIndex + delimiter.Length < text.Length &&
                char.IsWhiteSpace(text[openIndex + delimiter.Length]))
                return false;

            // Правило 3: Перед закрывающим должен идти непробельный символ
            if (closeIndex > 0 && char.IsWhiteSpace(text[closeIndex - 1]))
                return false;

            // Дополнительные правила для одинарного _ (не для __)
            if (!isDoubleUnderscore)
            {
                // _ не работает внутри цифр (_12_3)
                if (IsSurroundedByDigits(text, openIndex, closeIndex))
                    return false;

                // _ не работает между разными словами (ра_зных сл_овах)
                if (!IsInSameWord(text, openIndex, closeIndex))
                    return false;
            }

            return true;
        }

        private static bool IsSurroundedByDigits(string text, int openIndex, int closeIndex)
        {
            char charBeforeOpen = openIndex > 0 ? text[openIndex - 1] : ' ';
            char charAfterClose = closeIndex + 1 < text.Length ? text[closeIndex + 1] : ' ';

            return char.IsDigit(charBeforeOpen) && char.IsDigit(charAfterClose);
        }

        private static bool IsInSameWord(string text, int openIndex, int closeIndex)
        {
            int wordStart = openIndex;
            while (wordStart > 0 && !char.IsWhiteSpace(text[wordStart - 1]))
                wordStart--;

            int wordEnd = closeIndex + 1;
            while (wordEnd < text.Length && !char.IsWhiteSpace(text[wordEnd]))
                wordEnd++;

            return closeIndex < wordEnd;
        }
    }
}