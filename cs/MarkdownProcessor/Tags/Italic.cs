namespace MarkdownProcessor.Tags;

public class Italic : HighlightTextPairTag, ITag
{
    public Italic(Token openingToken) : base(openingToken)
    {
    }

    public override ITagMarkdownConfig Config { get; } = new ItalicConfig();

    public new void RunTagDownOfTree(ITag tag)
    {
        if (tag is Bold) return;
        base.RunTagDownOfTree(tag);
    }
}