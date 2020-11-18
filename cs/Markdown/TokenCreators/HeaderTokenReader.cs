namespace Markdown
{
    public class HeaderTokenReader : ITokenReader
    {
        public TextToken GetToken(string text, int index, int startPosition)
        {
            if (!CanCreateToken(text, index, startPosition))
                return null;

            var tokentText = text.Substring(1);

            return new TextToken(startPosition, text.Length,
                TokenType.Header, tokentText);
        }

        private static bool CanCreateToken(string text, int index, int startPosition)
        {
            return index == startPosition && text[startPosition] == '#'
                                          && index + 1 != text.Length && text[index + 1] != '\\';
        }
    }
}