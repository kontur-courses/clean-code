namespace Markdown.Tag;

public interface ITag
{
    string OpeningSeparator { get; }
    string CloseSeparator { get; }
    bool IsPaired { get; }
    void RenderParameters(string parametersValues, IList<string> parameters);
}