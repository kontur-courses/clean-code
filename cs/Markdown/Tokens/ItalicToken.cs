using Markdown.Parser;
using System.Collections.Generic;

namespace Markdown.Tokens
{
    public class ItalicToken : Token
    {
        public static readonly string Separator = "_";

        public override bool IsNonPaired => false;
        public override bool IsContented => false;
        public override string GetSeparator() => Separator;
        public ItalicToken(int openIndex) : base(openIndex) { }
        internal ItalicToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }

        internal override bool Validate(MdParser parser)
        {
            this.ValidatePlacedCorrectly(parser.TextToParse);
            ValidateItalicTokenInteractions(parser.Tokens, this);


            return IsCorrect;
        }

        private static void ValidateItalicTokenInteractions(IReadOnlyDictionary<string, Token> tokens, Token token)
        {
            if (!tokens.TryGetValue(BoldToken.Separator, out var boldToken)) return;
            if (!token.IsIntersectWith(boldToken)) return;

            boldToken.IsCorrect = false;
            token.IsCorrect = false;
        }
    }
}