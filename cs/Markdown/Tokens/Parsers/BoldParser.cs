namespace Markdown.Tokens.Parsers
{
    public class BoldParser: ITokenParser
    {
        public bool TryFindToken(string rawString, ref int position, out IToken token)
        {
            if (rawString[position] == '_' &&
                rawString.InBorders(position + 1) &&
                rawString[position + 1] == '_')
            {
                token = new BoldToken();
                position += 2;
                return true;
            }

            token = default;
            return false;
        }
    }
}