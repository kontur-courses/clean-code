namespace Markdown.Tokens
{
    public abstract class NonPairedToken : Token
    {
        protected NonPairedToken(int openIndex) : base(openIndex) { }
        protected NonPairedToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }
    }
}