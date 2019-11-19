using Markdown.Tokens;

namespace Markdown.Styles
{
    internal class Bold : Style
    {
        public Bold() : base(
            new Token[] { new Underline(), new Underline() }, 
            new Token[] { new Underline(), new Underline() },
            new StyleBeginToken(typeof(Bold)),
            new StyleEndToken(typeof(Bold))) { }
    }
}
