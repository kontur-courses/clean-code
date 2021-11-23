namespace Markdown
{
    public interface IToken
    {
        string Content
        { get; }
        bool IsNotToPairToken //лучше идей для названия не пришло :(
        { get; }
    }
}
