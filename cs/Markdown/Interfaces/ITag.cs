namespace Markdown.Interfaces
{
    public interface ITag : IWord
    {
        Tags Tag { get; }

        TagType TagType { get; }
    }

    public enum TagType
    {
        None,
        Open,
        Close,
        Single,
    }
}