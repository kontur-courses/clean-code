using Markdown.Tokens;

namespace Markdown.Lexer.ConcreteLexers
{
    public class CloseImageAltLexer : ConcreteLexer
    {
        public CloseImageAltLexer(LexContext context) : base(context)
        {
        }

        public override Token Lex() => GetDefaultTokenOrText('(', Token.CloseImageAlt);
    }
}