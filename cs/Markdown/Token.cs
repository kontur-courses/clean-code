namespace Markdown
{
    public static class Token
    {
        public static IToken Header1 => new TokenHeader1();
        public static IToken Italics => new TokenItalics();
        public static IToken Strong => new TokenStrong();
        public static IToken Escape => new TokenEscape();
        public static IToken NewLine => new TokenNewLine();
        public static IToken FromText(string value) => TokenText.FromText(value);
    }
}