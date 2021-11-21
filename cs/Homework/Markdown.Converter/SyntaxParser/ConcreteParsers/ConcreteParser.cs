namespace Markdown.SyntaxParser.ConcreteParsers
{
    internal abstract class ConcreteParser
    {
        protected readonly ParseContext Context;

        protected ConcreteParser(ParseContext context) => Context = context;

        public abstract TokenTree Parse();
    }
}