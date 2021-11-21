using Markdown.Tokens;

namespace Markdown.SyntaxParser.ConcreteParsers
{
    internal class ItalicsParser : UnderscoreParser
    {
        public ItalicsParser(ParseContext context) : base(context)
        {
        }

        public override TokenTree Parse()
            => ParseUnderscore(() =>
            {
                return Context.Current.TokenType switch
                {
                    TokenType.Bold => TokenTree.FromText("__"),
                    _ => Context.ParseToken()
                };
            });
    }
}