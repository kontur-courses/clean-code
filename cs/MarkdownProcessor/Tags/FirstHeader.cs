namespace MarkdownProcessor.Tags;

public class FirstHeader : Tag
{
    public FirstHeader(Token openingToken) : base(openingToken)
    {
    }

    public override ITagMarkdownConfig Config { get; } = new FirstHeaderConfig();


    protected override bool StopRunToken(Token token)
    {
        return false;
    }

    protected override bool StopRunTag(Tag tag)
    {
        return tag is not (Bold or Italic);
    }

    protected override bool IsClosingToken(Token token)
    {
        return token.Value == Config.ClosingSign;
    }
}