namespace Markdown
{
    public static class StringExtensions
    {
        public static Word GetWordContainingCurrentSymbol(this string text, int currentSymbolPointer)
        {
            var start = currentSymbolPointer;
            while (start != 0 && !char.IsWhiteSpace(text[start - 1])) start--;
            var length = 0;
            while (start + length < text.Length && !char.IsWhiteSpace(text[start + length])) length++;
            return new Word(text, start, length);
        }

        public static int GetEndOfParagraphPosition(this string text, int pointer)
        {
            while (!IsEndOfParagraph(text, pointer)) pointer++;
            return pointer;
        }

        public static bool IsStartOfParagraph(this string text, int pointer)
        {
            return pointer == 0 || text[pointer - 1] == '\n';
        }

        public static bool IsEndOfParagraph(this string text, int pointer)
        {
            return pointer == text.Length || text[pointer] == '\n';
        }
    }
}