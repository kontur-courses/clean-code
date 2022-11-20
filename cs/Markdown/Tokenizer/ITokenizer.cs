namespace Markdown
{
    public interface ITokenizer<TTag>
    {
        public IToken<TTag>[] Tokenize();
    }
}
