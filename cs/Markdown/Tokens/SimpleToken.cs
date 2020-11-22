namespace Markdown
{
    class SimpleToken : Token
    {
        public SimpleToken(int startPos = 0, int length = 0, Token parent = null) : base(startPos, length, parent)
        {
            Type = TokenType.Simple;
            OpeningTag = "";
            ClosingTag = "";
            ContainsOnlyDigits = true;
        }
    }
}
