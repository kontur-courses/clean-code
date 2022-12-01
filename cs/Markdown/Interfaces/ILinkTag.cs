namespace Markdown.Interfaces
{
    public interface ILinkTag : ITag
    {
        string Alternative { get; }

        string Title { get; }

        string Path { get; }

        bool TryParse(string context, int position, out ILinkTag linkTag);
    }
}