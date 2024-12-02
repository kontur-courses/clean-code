namespace Markdown.Tokens;

public class ParagraphToken : Token
{
    public override string TagWrapper => "h1";
    public override bool IsTag => true;
    
    public ParagraphToken(string content) : base(content) {}
    
    public override bool Validate(List<Token> tokens, int index) => true;
    public override bool IsOpen(List<Token> tokens, int index) => index == 0;
}