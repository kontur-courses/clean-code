namespace Markdown.Core.Tags
{
    interface ITag
    {
        string Opening { get; }
        string Closing { get; }
    }
}