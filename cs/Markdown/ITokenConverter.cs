namespace Markdown
{
    public interface ITokenConverter
    {
        string ConvertToken(FormattedToken formattedToken, string text);
    }
}
