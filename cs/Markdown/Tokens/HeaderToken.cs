namespace Markdown
{
    class HeaderToken : Token
    {
        public HeaderToken(int startPos = 0, int length = 0, IToken parent = null) : base(startPos, length, parent)
        {
            {
                Type = TokenType.Header;
                OpeningTag = "# ";
                ClosingTag = "";
                HtmlTag = "<h1>";
                NestingLevel = 1;
            }
        }
    }
}
