using Markdown.Tokens;

namespace Markdown.SyntaxParser.ConcreteParsers
{
    internal class EscapeParser : ConcreteParser
    {
        public EscapeParser(ParseContext context) : base(context)
        {
        }

        public override TokenTree Parse()
        {
            if (Context.IsEndOfFile())
                return TokenTree.FromText("\\");

            switch (Context.LookAhead.TokenType)
            {
                case TokenType.Bold:
                case TokenType.Italics:
                case TokenType.Header1:
                    Context.MoveToNextToken();
                    return TokenTree.FromText(Context.Current.Value);
                case TokenType.Escape:
                    Context.MoveToNextToken();
                    break;
            }

            return TokenTree.FromText("\\");
        }
    }
}