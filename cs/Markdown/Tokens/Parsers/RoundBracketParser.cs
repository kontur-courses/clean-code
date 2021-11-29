namespace Markdown.Tokens.Parsers
{
    public class RoundBracketParser: ITokenParser
    {
        public bool TryFindToken(string rawString, ref int position, out IToken token)
        {
            switch (rawString[position])
            {
                case '(':
                    token = new OpeningRoundBracketToken();
                    position += 1;
                    return true;
                case ')':
                    token = new ClosingRoundBracketToken();
                    position += 1;
                    return true;
                default:
                    token = default;
                    return false;
            }
        }
    }
}