namespace Markdown
{
    public interface IMarkProcessor
    {
        public TokenMd FormatToken(TokenMd token);
    }
}