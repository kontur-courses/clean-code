using Markdown.Tokens;

namespace Markdown.SyntaxParser
{
    public class ParseHelper
    {
        private readonly ParseContext context;

        public ParseHelper(ParseContext context) => this.context = context;

        public bool TryGetOffsetOfFirstTagAppearanceInLine(TokenType tokenType, out int offset, int startOffset = 1)
        {
            offset = startOffset;
            do
            {
                var currentToken = context.Peek(offset);
                if (currentToken.TokenType == tokenType)
                    return true;
                offset++;
            } while (!context.IsEndOfFileOrNewLine(offset));

            return false;
        }
    }
}