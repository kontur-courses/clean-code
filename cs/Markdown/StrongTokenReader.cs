namespace Markdown
{
    public class StrongTokenReader : ITokenReader
    {
        public Token? TryReadToken(string text, int index)
        {
            if (!IsStrongStartTag(text, index))
                return null;

            var value = "";
            for (var i = index + 2; i < text.Length; i++)
            {
                if (!IsStrongEndTag(text, i))
                    continue;

                value = text[index..(i + 2)];
                break;
            }

            return value != "" ? new Token(index, value, TokenType.Strong) : null;
        }

        private static bool IsStrongStartTag(string text, int index)
        {
            return index >= 0
                   && text[index] == '_'
                   && index + 2 < text.Length
                   && text[index + 1] == '_'
                   && !text[index + 2].IsDigitOrWhiteSpace()
                   && text[index + 2] != '_'
                   && !new TextParser().IsAfterBackslash(text, index);
        }

        private static bool IsStrongEndTag(string text, int index)
        {
            return text[index] == '_'
                   && !text[index - 1].IsDigitOrWhiteSpace()
                   && text[index - 1] != '_'
                   && (index + 1 == text.Length || text[index + 1] == '_')
                   && !new TextParser().IsAfterBackslash(text, index);
        }
    }
}