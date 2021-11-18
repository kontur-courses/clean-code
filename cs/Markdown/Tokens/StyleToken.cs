namespace Markdown.Tokens
{
    public abstract class StyleToken : Token
    {
        protected StyleToken(int openIndex) : base(openIndex) { }
        protected StyleToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }
    }
}