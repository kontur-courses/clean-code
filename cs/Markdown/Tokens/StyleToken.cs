namespace Markdown.Tokens
{
    internal abstract class StyleToken : Token
    {
        protected StyleToken(int openIndex) : base(openIndex) { }
    }
}