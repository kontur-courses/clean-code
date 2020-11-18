namespace Markdown
{
    public class EscapedStringToken : Token
    {
        public EscapedStringToken() : this(0, 2, null)
        {
        }

        public EscapedStringToken(int startPosition, int length = 2, Token parent = null)
            : base(startPosition, length, parent)
        {
        }

        public string GetEscapedString(string text) => text.Substring(StartPosition + 1, Length - 1);
    }
}