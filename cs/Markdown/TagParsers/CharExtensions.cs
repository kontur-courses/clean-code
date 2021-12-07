namespace Markdown.TagParsers
{
    public static class CharExtensions
    {
        public static bool IsLetterOrPunctuationButNotTag(this char c)
        {
            return char.IsLetter(c) || c.IsPunctuationButNotTag();
        }

        public static bool IsPunctuationButNotTag(this char c)
        {
            return char.IsPunctuation(c) && !c.IsTagSymbol();
        }

        public static bool IsTagSymbol(this char c)
        {
            return c == '#' || c == ' ' || c == '\n' || c == '_' || c == '\\';
        }

        public static bool IsWhitespaceButNotNewLine(this char c)
        {
            return c == ' ';
        }
    }
}
