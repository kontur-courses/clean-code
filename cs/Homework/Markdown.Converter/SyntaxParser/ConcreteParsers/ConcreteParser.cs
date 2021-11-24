using System.Text;
using Markdown.Tokens;

namespace Markdown.SyntaxParser.ConcreteParsers
{
    internal abstract class ConcreteParser
    {
        protected readonly ParseContext Context;

        protected ConcreteParser(ParseContext context) => Context = context;

        public abstract TokenTree Parse();

        protected int GetOffsetOfFirstTagAppearanceInLine(TokenType tokenType, int startOffset = 1)
        {
            var i = startOffset;
            Token currentToken;
            do
            {
                currentToken = Context.Peek(i);
                if (currentToken.TokenType == tokenType)
                    return i;
                i++;
            } while (currentToken.TokenType != TokenType.NewLine && Context.Position + i != Context.Tokens.Length);

            return -1;
        }

        protected string ParseToText(int count, string prefix = "")
        {
            var result = new StringBuilder(prefix);
            for (var i = 0; i < count; i++)
            {
                Context.NextToken();
                result.Append(Context.Current.Value);
            }

            return result.ToString();
        }
    }
}