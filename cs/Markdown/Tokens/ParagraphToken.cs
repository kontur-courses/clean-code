namespace Markdown.Tokens;

public class ParagraphToken : TextContainerToken
{
    public ParagraphToken(Token parent, string value) : base(parent, value)
    {
    }

    public override TokenType Type => TokenType.Paragraph;
}