namespace Markdown.Tags;

public class Bulleted : Tag
{
    protected override Tag CreateTag(string content, Token? previousToken, string nextChar)
    {
        IsPaired = false;
        ReplacementForOpeningTag = "<li>";
        ReplacementForClosingTag = "</li>";
        TagType = TagType.Heading;
        TagContent = content;
        Status = TagStatus.SelfClosing;
        BlockToken(previousToken);
        return this;
    }

    private void BlockToken(Token? previousToken)
    {
        if (previousToken != null)
        {
            Status = TagStatus.Block;
        }
    }
}