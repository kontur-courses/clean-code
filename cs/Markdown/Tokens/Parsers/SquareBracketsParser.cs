namespace Markdown.Tokens.Parsers
{
    public class SquareBracketsParser: ITokenParser
    {
        public bool TryFindToken(string rawString, ref int position, out IToken token)
        {
            switch (rawString[position])
            {
                case '[':
                    token = new OpeningSquareBracketToken();
                    position += 1;
                    return true;
                case ']':
                    token = new ClosingSquareBracketToken();
                    position += 1;
                    return true;
                default:
                    token = default;
                    return false;
            }

        }
    }
}