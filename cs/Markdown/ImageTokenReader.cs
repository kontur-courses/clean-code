namespace Markdown
{
    public class ImageTokenReader : ITokenReader
    {
        public Token? TryReadToken(string text, int index)
        {
            if (!IsImageTagStart(text, index))
                return null;

            var value = "";
            for (var i = index + 1; i < text.Length; i++)
            {
                if (!IsImageTagEnd(text, i))
                    continue;

                value = text[index..(i + 1)];
                break;
            }

            return value != "" ? new Token(index, value, TokenType.Image) : null;
        }

        private static bool IsImageTagStart(string text, int index)
        {
            return text[index] == '!'
                   && (index + 1 == text.Length || text[index + 1] == '[');
        }

        private static bool IsImageTagEnd(string text, int index)
        {
            return text[index] == ')';
        }
    }
}