namespace Markdown.Tags.Implementation;

public class Header : ITag
{
    public string SourceName => "#";
    public string TranslateName => "!!!Header!!!";
    public string ResultName => "h1";
}