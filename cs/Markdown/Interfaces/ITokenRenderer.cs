namespace Markdown.Interfaces
{
    public interface ITokenRenderer
    {
        string Render(TokenTree[] trees);
    }
}