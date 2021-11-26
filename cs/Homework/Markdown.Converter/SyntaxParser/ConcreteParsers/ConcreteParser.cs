using System.Text;

namespace Markdown.SyntaxParser.ConcreteParsers
{
    internal abstract class ConcreteParser
    {
        protected readonly ParseContext Context;

        protected ConcreteParser(ParseContext context) => Context = context;

        public abstract TokenTree Parse();

        protected string ParseToText(int count, string prefix = "")
        {
            var result = new StringBuilder(prefix);

            for (var i = 0; i < count; i++)
            {
                Context.MoveToNextToken();
                result.Append(Context.Current.Value);
            }

            return result.ToString();
        }
    }
}