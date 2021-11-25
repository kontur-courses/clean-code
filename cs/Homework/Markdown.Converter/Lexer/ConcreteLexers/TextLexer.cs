using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Lexer.ConcreteLexers
{
    public class TextLexer : ConcreteLexer
    {
        private readonly HashSet<char> specSymbols;

        public TextLexer(HashSet<char> specSymbols, LexContext context) : base(context)
            => this.specSymbols = specSymbols;

        public override Token Lex()
        {
            var buffer = new StringBuilder();

            while (!specSymbols.Contains(Context.Current) && Context.Current != LexContext.EndOfFile)
            {
                buffer.Append(Context.Current);
                Context.MoveToNextSymbol();
            }

            Context.MoveToPreviousSymbol();

            return Token.Text(buffer.ToString());
        }
    }
}