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
            var closingTagOffset = GetOffsetOfFirstTagAppearanceInLine(TokenType.CloseImageAlt);
            if (closingTagOffset == -1)
                return TokenTree.FromText(Context.Current.Value);
            var closingImageOffset = GetOffsetOfFirstTagAppearanceInLine(TokenType.CloseBracket);
            if (closingImageOffset == -1)
                return TokenTree.FromText(Context.Current.Value);

            var altPart = ParseToText(closingTagOffset - 1);
            Context.NextToken();
            var srcPart = ParseToText(closingImageOffset - closingTagOffset - 1);
            Context.NextToken();

            var token = new ImageToken(srcPart, altPart);
            return new TokenTree(token);
        }
    }
}