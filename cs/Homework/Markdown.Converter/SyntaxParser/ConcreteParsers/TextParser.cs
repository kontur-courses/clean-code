namespace Markdown.SyntaxParser.ConcreteParsers
{
    internal class TextParser : ConcreteParser
    {
        public TextParser(ParseContext context) : base(context)
        {
        }

        public override TokenTree Parse() => TokenTree.FromText(Context.Current.Value);
    }
}