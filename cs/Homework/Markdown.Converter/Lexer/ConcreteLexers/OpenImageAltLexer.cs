using Markdown.Tokens;

namespace Markdown.Lexer.ConcreteLexers
{
    public class OpenImageAltLexer : ConcreteLexer
    {
        public OpenImageAltLexer(LexContext context) : base(context)
        {
        }

        public override Token Lex() => GetDefaultTokenOrText('[', Token.OpenImageAlt);
    }
}