namespace Markdown.Tags;

public interface ITag
{
    public string SourceName { get; }
    public string TranslateName { get; }
    public string ResultName { get; }
    public string MakeTransformations(string substring);
}