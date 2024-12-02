namespace Markdown.Tokens;

public class ScreeningToken : Token
{
    public override string TagWrapper => null;
    public override bool IsTag => false;
    
    public ScreeningToken(string content) : base(content) {}
    
    public override bool Validate(List<Token> tokens, int index) => true;
    
    public override bool IsOpen(List<Token> tokens, int index) => true;
}