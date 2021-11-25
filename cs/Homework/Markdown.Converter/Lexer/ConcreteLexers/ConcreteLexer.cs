using Markdown.Tokens;

namespace Markdown.Lexer.ConcreteLexers
{
    public abstract class ConcreteLexer
    {
        protected ConcreteLexer(LexContext context) => Context = context;
        protected LexContext Context { get; }

        public abstract Token Lex();

        protected Token GetDefaultTokenOrText(char lookaheadShould, Token defaultToken)
        {
            if (Context.Lookahead != lookaheadShould)
                return Token.Text(Context.Current.ToString());
            Context.MoveToNextSymbol();
            return defaultToken;
        }
    }
}