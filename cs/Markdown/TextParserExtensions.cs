using System.Collections.Generic;

namespace Markdown
{
    public static class TextParserExtensions
    {
        private static bool IsAfterBackslash(this TextParser parser, int index)
        {
            return index != 0 && parser.EscapingBackslashes.Contains(index - 1);
        }

        public static bool IsEmphasizedStartTag(this TextParser parser, string text, int index)
        {
            return text[index] == '_'
                   && index + 1 < text.Length
                   && text[index + 1] != '_'
                   && !text[index + 1].IsDigitOrWhiteSpace()
                   && !parser.IsAfterBackslash(index)
                   && !IsStrongStartTag(parser, text, index - 1);
        }

        public static bool IsEmphasizedEndTag(this TextParser parser, string text, int index)
        {
            return text[index] == '_'
                   && !text[index - 1].IsDigitOrWhiteSpace()
                   && text[index - 1] != '_'
                   && (index + 1 == text.Length || text[index + 1] != '_')
                   && !parser.IsAfterBackslash(index);
        }

        public static bool IsStrongStartTag(this TextParser parser, string text, int index)
        {
            return index >= 0
                   && text[index] == '_'
                   && index + 2 < text.Length
                   && text[index + 1] == '_'
                   && !text[index + 2].IsDigitOrWhiteSpace()
                   && text[index + 2] != '_'
                   && !parser.IsAfterBackslash(index);
        }

        public static bool IsStrongEndTag(this TextParser parser, string text, int index)
        {
            return text[index] == '_'
                   && !text[index - 1].IsDigitOrWhiteSpace()
                   && text[index - 1] != '_'
                   && index + 1 < text.Length && text[index + 1] == '_'
                   && !parser.IsAfterBackslash(index);
        }

        public static bool IsPlainText(this TextParser parser, string text, int index)
        {
            return !parser.IsStrongStartTag(text, index)
                   && !parser.IsEmphasizedStartTag(text, index)
                   && text[index] != '#';
        }
    }
}