namespace Markdown.Tokens.Parsers
{
    public class CharParser: ITokenParser
    {
        public bool TryFindToken(string rawString, ref int position, out IToken token)
        {
            token = new CharToken(rawString[position]);
            position += 1;
            return true;
        }
    }
}