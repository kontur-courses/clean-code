using Markdown.Tokens;

namespace Markdown.Lexer.ConcreteLexers
{
    public class UnderscoreLexer : ConcreteLexer
    {
        public UnderscoreLexer(LexContext context) : base(context)
        {
        }

        public override Token Lex()
        {
            if (Context.Lookahead != '_')
                return Token.Italics;
            Context.MoveToNextSymbol();
            return Token.Bold;
        }
    }
}