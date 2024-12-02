namespace Markdown.Tokens;

public class SpaceToken : Token
{
    public override string TagWrapper => null;
    public override bool IsTag => false;
    
    public SpaceToken(string content) : base(content) {}
    
    public override bool Validate(List<Token> tokens, int index) => true;
    
    public override bool IsOpen(List<Token> tokens, int index) => true;
}