namespace Markdown
{
    public interface ITokenizer
    {
        public Token[] Tokenize(string text);
    }
}