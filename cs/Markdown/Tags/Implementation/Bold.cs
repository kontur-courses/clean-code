namespace Markdown.Tags.Implementation;

public class Bold : ITag
{
    public string BeforeTranslateName => "__";
    public string TranslateName => "!!!Bold!!!";
    public string AfterTranslateName => "strong";
}