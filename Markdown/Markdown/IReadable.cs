namespace Markdown
{
    public interface IReadable
    {
        void AddNested(IReadable readable);
        string MakeToHtml(Token token);
    }
}