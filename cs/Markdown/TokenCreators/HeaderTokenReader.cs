namespace Markdown
{
    public class HeaderTokenReader : ITokenReader
    {
        public TextToken TyrGetToken(string text, int index, int startPosition)
        {
            if (!CanCreateToken(text, index, startPosition))
                return null;
            

            return new HeaderTextToken(text);
        }

        private static bool CanCreateToken(string text, int index, int startPosition)
        {
            return index == startPosition && text[startPosition] == '#'
                                          && index + 1 != text.Length && text[index + 1] != '\\';
        }
    }
}