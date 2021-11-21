namespace Markdown.SyntaxParser.ConcreteParsers
{
    internal class BoldParser : UnderscoreParser
    {
        public BoldParser(ParseContext context) : base(context)
        {
        }

        public override TokenTree Parse() => ParseUnderscore(Context.ParseToken);
    }
}