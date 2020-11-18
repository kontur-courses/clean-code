namespace Markdown
{
    public interface ITagTokenConverter
    {
        string ConvertToken(TextToken token);
    }
}