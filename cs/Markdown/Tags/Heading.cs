namespace Markdown.Tags;

public class Heading : Tag
{
    protected override Tag CreateTag(string content, Token? previousToken, string nextChar)
    {
        IsPaired = false;
        var nestingLevel = GetNestingLevel(content);
        ReplacementForOpeningTag = $"<h{nestingLevel}>";
        ReplacementForClosingTag = $"</h{nestingLevel}>";
        TagType = TagType.Heading;
        TagContent = content;
        Status = TagStatus.Block;
        BlockToken(previousToken);
        return this;
    }

    protected virtual void BlockToken(Token? previousToken)
    {
        if (previousToken == null || (previousToken.Tag != null && previousToken.Tag.TagType != TagType.Bulleted))
            Status = TagStatus.SelfClosing;
    }

    private static int GetNestingLevel(string content)
    {
        return content.Length - 1;
    }
}