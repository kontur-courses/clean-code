namespace Markdown.Core.Tags
{
    interface IDoubleTag : ITag
    {
        string Opening { get; }
        string Closing { get; }
    }
}