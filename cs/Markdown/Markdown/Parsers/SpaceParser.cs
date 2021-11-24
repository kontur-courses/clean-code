namespace Markdown.Parsers
{
    internal class SpaceParser : IParser
    {
        public IToken TryGetToken()
        {
            return new TokenSpace();
        }
    }
}