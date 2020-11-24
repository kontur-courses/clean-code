namespace Markdown.Tokens
{
    public class EmphasizedToken : Token
    {
        public EmphasizedToken(int position, string value, int endPosition)
            : base(position, value, endPosition, TokenType.Emphasized, true)
        {
        }
    }
}