namespace MarkdownProcessor.Tags;

public class Italic : HighlightTextPairTag
{
    public Italic(Token openingToken) : base(openingToken)
    {
    }

    public override ITagMarkdownConfig Config { get; } = new ItalicConfig();

    protected override bool ForbiddenChild(Tag tag)
    {
        return tag is Bold;
    }
}