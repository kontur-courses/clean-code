namespace Markdown.Tokens.Parsers
{
    public interface ITokenParser
    {
        public bool TryFindToken(string rawString, ref int position, out IToken token);
    }
}