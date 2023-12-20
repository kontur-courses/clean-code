namespace Markdown.Tags;

public class Heading : Tag
{
    protected override Tag CreateTag(string content, Token previousToken, string nextChar)
    {
        IsPaired = false;
        ConvertTo = "<h1>";
        ClosingTag = "</h1>";
        TokenType = TagType.Heading;
        Content = content;
        Status = TagStatus.Block;
        PreviousToken = previousToken;
        BlockToken();
        return this;
    }

    protected virtual void BlockToken()
    {
        if (PreviousToken == null || (PreviousToken.Tag != null && PreviousToken.Tag.TokenType != TagType.Bulleted))
        {
            Status = TagStatus.SelfClosing;
        }
    }
    
    
}