using Markdown.Tokens;
using Markdown.Tokens.ConcreteTokens;

namespace Markdown.SyntaxParser.ConcreteParsers
{
    internal class ImageParser : ConcreteParser
    {
        public ImageParser(ParseContext context) : base(context)
        {
        }

        public override TokenTree Parse()
        {
            if (!ParseHelper.TryGetOffsetOfFirstTagAppearanceInLine(Context, TokenType.CloseImageAlt, out var closingTagOffset))
                return TokenTree.FromText(Context.Current.Value);

            if (!ParseHelper.TryGetOffsetOfFirstTagAppearanceInLine(Context, TokenType.CloseBracket, out var closingImageOffset))
                return TokenTree.FromText(Context.Current.Value);

            var altPart = ParseToText(closingTagOffset - 1);
            Context.MoveToNextToken();

            var srcPart = ParseToText(closingImageOffset - closingTagOffset - 1);
            Context.MoveToNextToken();

            var token = new ImageToken(srcPart, altPart);
            return new TokenTree(token);
        }
    }
}