using Markdown.Enums;

namespace Markdown.Tokens;

public class LiteralToken : Token
{
    public override string TagWrapper => null;
    public override bool IsTag => false;

    public LiteralType ContentType { get; }

    public LiteralToken(string content, LiteralType contentType) : base(content)
    {
        ContentType = contentType;
    }

    public LiteralToken(Token token) : base(token)
    {
        ContentType = LiteralType.Text;
    }

    public override bool Validate(List<Token> tokens, int index) => true;
    
    public override bool IsOpen(List<Token> tokens, int index) => true;

    public bool IsNumber() => ContentType == LiteralType.Number;
}