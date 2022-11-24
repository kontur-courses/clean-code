namespace Markdown.Tags.Implementation;

public class Italy : ITag
{
    public string SourceName => "_";
    public string TranslateName => "!!!Italy!!!";
    public string ResultName => "em";
    public string MakeTransformations(string substring) => substring;
}