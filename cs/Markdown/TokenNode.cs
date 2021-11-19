namespace Markdown
{
    public readonly struct TokenNode
    {
        public readonly Token Token;
        public readonly Token? Parent;

        public TokenNode(Token token, Token? parent)
        {
            Token = token;
            Parent = parent;
        }
    }
}