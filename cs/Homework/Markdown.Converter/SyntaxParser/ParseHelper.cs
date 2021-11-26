using Markdown.Tokens;

namespace Markdown.SyntaxParser
{
    public static class ParseHelper
    {
        public static bool TryGetOffsetOfFirstTagAppearanceInLine(
            ParseContext context, 
            TokenType tokenType, 
            out int offset, 
            int startOffset = 1)
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
        
        public static bool TryGetIntersectionIndex(ParseContext context, TokenType tokenType, out int index)
        {
            index = 0;
            if (!ParseHelper.TryGetOffsetOfFirstTagAppearanceInLine(context, context.Current.TokenType, out var closingTagIndex))
                return false;
            if (!ParseHelper.TryGetOffsetOfFirstTagAppearanceInLine(context, tokenType, out var otherOpeningTagIndex))
                return false;
            if (!ParseHelper.TryGetOffsetOfFirstTagAppearanceInLine(context, tokenType, 
                out index, otherOpeningTagIndex + 1))
                return false;

            return otherOpeningTagIndex < closingTagIndex && index > closingTagIndex;
        }
        
        public static bool ContainsWordsThatSeparatedBy(ParseContext context, char symbol)
        {
            var i = 1;
            Token currentToken;
            do
            {
                currentToken = context.Peek(i);
                var value = currentToken.Value;
                if (value.Contains(symbol))
                    return true;
                i++;
            } while (!context.IsEndOfFileOrNewLine() && currentToken.TokenType != context.Current.TokenType);

            return false;
        }
    }
}