namespace Markdown.Core.Tags
{
    interface ISingleTag : ITag
    {
        string Opening { get; }
    }
}