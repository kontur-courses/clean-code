namespace Markdown.Tags;

public class Bulleted : Heading
{
    protected override Tag CreateTag(string content, Token previousToken, string nextChar)
    {
        IsPaired = false;
        ConvertTo = "<li>";
        ClosingTag = "</li>";
        TokenType = TagType.Heading;
        Content = content;
        Status = TagStatus.SelfClosing;
        PreviousToken = previousToken;
        BlockToken();
        return this;
    }

    protected override void BlockToken()
    {
        if (PreviousToken != null)
        {
            Status = TagStatus.Block;
        }
    }
}