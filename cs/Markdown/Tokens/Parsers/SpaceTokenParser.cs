namespace Markdown.Tokens.Parsers
{
    public class SpaceTokenParser: ITokenParser
    {
        public bool TryFindToken(string rawString, ref int position, out IToken token)
        {
            if (rawString[position] == ' ')
            {
                position += 1;
                token = new SpaceToken();
                return true;
            }

            token = default;
            return false;
        }
    }
}