namespace Markdown
{
    public interface ITagTokenConverter
    {
        string ConvertToken(IToken token);
    }
}