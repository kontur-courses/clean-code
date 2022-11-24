namespace Markdown.Tags.Implementation;

public class Bold : ITag
{
    public string SourceName => "__";
    public string TranslateName => "!!!Bold!!!";
    public string ResultName => "strong";
    public string MakeTransformations(string substring) => substring;
}