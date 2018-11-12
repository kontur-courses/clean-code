namespace Markdown
{
    public class Token
    {
        public int Position;
        public int Length;
        //public string Text;
        public ITokenType Type;
        public Token[] InnerTokens;

        public string ToHtml(string mdText)
        {
            var text = GetText(mdText);
            return Type == null ? text : Type.ToHtml(text);
        }

        public string GetText(string mdText) => mdText.Substring(Position, Length);
    }
}
