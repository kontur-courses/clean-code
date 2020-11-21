namespace Markdown
{
    public class ImageTokenReader : ITokenReader
    {
        public bool TryReadToken(string text, string context, int index, out Token? token)
        {
            var altText = "";
            var url = "";
            token = null;

            if (!IsImageTagStart(text, index))
                return false;

            var value = "";
            for (var i = index + 1; i < text.Length; i++)
            {
                if (IsAltTexEnd(text, i) && text[i - 1] != '[')
                    altText = text[2..i];

                if (IsImageTagEnd(text, i) && text[i - 1] != '(')
                    url = text[(altText.Length + 4)..^1];

                if (text[i] == ']')
                    if (!IsAltTexEnd(text, i))
                        return false;

                if (!IsImageTagEnd(text, i))
                    continue;

                value = text[index..(i + 1)];
                break;
            }

            if (value == "")
                return false;

            token = new Token(index, value, index + value.Length - 1, TokenType.Image);
            token.ChildTokens.Add(new Token(0, altText, 0, TokenType.PlainText));
            token.ChildTokens.Add(new Token(1, url, 1, TokenType.PlainText));
            return true;
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

        private static bool IsAltTexEnd(string text, int index)
        {
            return text[index] == ']'
                   && (index + 1 == text.Length || text[index + 1] == '(');
        }
    }
}