namespace Markdown.Tokens.Parsers
{
    public class ItalicParser : ITokenParser
    {
        public bool TryFindToken(string rawString, ref int position, out IToken token)
        {
            if (rawString[position] == '_')
            {
                position += 1;
                token = new ItalicToken();
                return true;
            }

            token = default;
            return false;
        }
    }
}