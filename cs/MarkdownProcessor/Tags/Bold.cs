namespace MarkdownProcessor.Tags;

public class Bold : HighlightTextPairTag
{
    public Bold(Token openingToken) : base(openingToken)
    {
    }

    public override ITagMarkdownConfig Config { get; } = new BoldConfig();

    protected override bool ForbiddenChild(Tag tag)
    {
        return false;
    }
}