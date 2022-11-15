namespace Markdown
{
    public class CommonToken : Token
    {
        public string text;

        public CommonToken(string text, int startIndex)
        {
            this.text = text;
            Length = text.Length;
        }
    }
}
