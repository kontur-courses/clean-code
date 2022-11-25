namespace MarkdownProcessor.Markdown;

public class Bold : HighlightTextPairTag, ITag
{
    public Bold(Token openingToken) : base(openingToken)
    {
    }

    public override ITagMarkdownConfig Config { get; } = new BoldConfig();
}