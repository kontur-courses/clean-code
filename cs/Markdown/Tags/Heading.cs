namespace Markdown.Tags;

public class Heading : Tag
{
    protected override Tag CreateTag(string content, Token previousToken, string nextChar)
    {
        IsPaired = false;
        ReplacementForOpeningTag = "<h1>";
        ReplacementForClosingTag = "</h1>";
        TagType = TagType.Heading;
        TagContent = content;
        Status = TagStatus.Block;
        BlockToken(previousToken);
        return this;
    }

    protected virtual void BlockToken(Token previousToken)
    {
        if (previousToken == null || (previousToken.Tag != null && previousToken.Tag.TagType != TagType.Bulleted))
        {
            Status = TagStatus.SelfClosing;
        }
    }
    
    
}