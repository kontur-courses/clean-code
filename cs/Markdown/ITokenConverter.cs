namespace Markdown
{
    public interface ITokenConverter
    {
        string Convert(IToken token);
    }
}