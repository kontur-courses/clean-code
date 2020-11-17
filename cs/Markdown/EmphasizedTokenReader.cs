namespace Markdown
{
    public class EmphasizedTokenReader : ITokenReader
    {
        public Token? TryReadToken(string text, int index)
        {
            if (!IsEmphasizedStartTag(text, index))
                return null;

            var value = "";
            for (var i = index + 1; i < text.Length; i++)
            {
                if (!IsEmphasizedEndTag(text, i))
                    continue;

                value = text[index..(i + 1)];
                break;
            }

            return value != "" ? new Token(index, value, TokenType.Emphasized) : null;
        }

        private static bool IsEmphasizedStartTag(string text, int index)
        {
            return text[index] == '_'
                   && index + 1 < text.Length
                   && text[index + 1] != '_'
                   && !text[index + 1].IsDigitOrWhiteSpace()
                   && (index - 1 < 0 || text[index - 1] != '_')
                   && !new TextParser().IsAfterBackslash(text, index);
        }

        private static bool IsEmphasizedEndTag(string text, int index)
        {
            return text[index] == '_'
                   && index - 1 >= 0
                   && !text[index - 1].IsDigitOrWhiteSpace()
                   && text[index - 1] != '_'
                   && (index + 1 == text.Length || text[index + 1] != '_')
                   && !new TextParser().IsAfterBackslash(text, index);
        }
    }
}