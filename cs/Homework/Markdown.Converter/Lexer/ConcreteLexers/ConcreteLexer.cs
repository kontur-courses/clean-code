using Markdown.Tokens;

namespace Markdown.Lexer.ConcreteLexers
{
    public abstract class ConcreteLexer
    {
        protected ConcreteLexer(LexContext context) => Context = context;
        protected LexContext Context { get; }

        public abstract Token Lex();
    }
}