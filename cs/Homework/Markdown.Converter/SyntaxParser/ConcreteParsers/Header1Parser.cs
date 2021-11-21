using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.SyntaxParser.ConcreteParsers
{
    internal class Header1Parser : ConcreteParser
    {
        public Header1Parser(ParseContext context) : base(context)
        {
        }

        public override TokenTree Parse()
        {
            if (Context.Previous.TokenType != TokenType.NewLine && Context.Position != 0)
                return TokenTree.FromText(Context.Current.Value);
            if (Context.Position + 1 == Context.Tokens.Length)
                return TokenTree.FromText(string.Empty);
            var buffer = new List<TokenTree>();
            do
            {
                Context.NextToken();
                buffer.Add(Context.ParseToken());
            } while (!Context.IsEndOfFileOrNewLine());

            return new TokenTree(Token.Header1, buffer.ToArray());
        }
    }
}