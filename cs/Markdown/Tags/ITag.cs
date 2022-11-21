namespace Markdown.Tags;

public interface ITag
{
    public string BeforeTranslateName { get; }
    public string TranslateName { get; }
    public string AfterTranslateName { get; }
}