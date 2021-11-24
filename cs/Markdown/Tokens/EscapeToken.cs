namespace Markdown.Tokens
{
    public class EscapeToken : Token
    {
        public EscapeToken(string content) : base(content)
        {
        }

        protected override bool AllowInners => false;
        public override string Render()
        {
            return Content.Substring(1, Content.Length -1);
        }
    }
}
