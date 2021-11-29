namespace Markdown.Tokens.Parsers
{
    public class EndLineParser: ITokenParser
    {
        public bool TryFindToken(string rawString, ref int position, out IToken token)
        {
            if (rawString[position] == '\n')
            {
                token = new EndLineToken();
                position += 1;
                return true;
            }

            token = default;
            return false;
        }
    }
}