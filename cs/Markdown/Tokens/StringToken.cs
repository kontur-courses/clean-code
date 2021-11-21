namespace Markdown
{
    public class StringToken : Token
    {
        public override int RenderDelta => 0;
        public override bool AllowInners => false;

        public StringToken(int begin, int end) : base(begin, end)
        {
        }

        public override string Render(string str)
        {
            return str.Substring(Begin, Length);
        }
    }
}
