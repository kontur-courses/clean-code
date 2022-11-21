namespace Markdown.Tags.Implementation;

public class Header : ITag
{
    public string BeforeTranslateName => "#";
    public string TranslateName => "!!!Header!!!";
    public string AfterTranslateName => "h1";
}