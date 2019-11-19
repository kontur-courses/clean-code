using Markdown.Tokens;

namespace Markdown.Styles
{
    internal class Italic : Style
    {
        public Italic() : base(
            new Token[] { new Underline() }, 
            new Token[] { new Underline() },
            new StyleBeginToken(typeof(Italic)),
            new StyleEndToken(typeof(Italic))) { }
    }
}
