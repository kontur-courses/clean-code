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
            if (!Context.IsStartOfFileOrNewLine())
                return TokenTree.FromText(Context.Current.Value);

            if (Context.IsEndOfFile())
                return TokenTree.FromText(string.Empty);

            var buffer = new List<TokenTree>();
            do
            {
                Context.MoveToNextToken();
                buffer.Add(Context.ParseToken());
            } while (!Context.IsEndOfFileOrNewLine());

            return new TokenTree(Token.Header1, buffer.ToArray());
        }
    }
}