namespace Markdown.Tokens.Parsers
{
    public class Header1Parser: ITokenParser
    {
        public bool TryFindToken(string rawString, ref int position, out IToken token)
        {
            if (rawString[position] == '#')
            {
                token = new Header1Token();
                position += 1;
                return true;
            }

            token = default;
            return false;
        }
    }
}