namespace Markdown
{
    public interface IMarkParser
    {
        public TokenMd GetToken(string text, int index, out int finalIndex);
    }
}