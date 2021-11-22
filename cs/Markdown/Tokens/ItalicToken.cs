using System.Collections.Generic;
using Markdown.Parser;

namespace Markdown.Tokens
{
    public class ItalicToken : Token
    {
        public const string Separator = "_";

        public override bool IsNonPaired => false;
        public override bool IsContented => false;
        public ItalicToken(int openIndex) : base(openIndex) { }
        internal ItalicToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }

        public override string GetSeparator()
        {
            return Separator;
        }

        internal override bool Validate(MdParser parser)
        {
            this.ValidatePlacedCorrectly(parser.TextToParse);
            ValidateInteractions(parser.Tokens, this);

            return IsCorrect;
        }

        private static void ValidateInteractions(IReadOnlyDictionary<string, Token> tokens, Token token)
        {
            if (!tokens.TryGetValue(BoldToken.Separator, out var boldToken)) return;
            if (!token.IsIntersectWith(boldToken)) return;

            boldToken.IsCorrect = false;
            token.IsCorrect = false;
        }
    }
}