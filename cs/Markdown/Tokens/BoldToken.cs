using System.Collections.Generic;
using Markdown.Parser;

namespace Markdown.Tokens
{
    public class BoldToken : Token
    {
        public const string Separator = "__";

        public override bool IsNonPaired => false;
        public override bool IsContented => false;
        public BoldToken(int openIndex) : base(openIndex) { }
        internal BoldToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }

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
            if (!token.IsCorrect || !tokens.TryGetValue(ItalicToken.Separator, out var italicToken))
                return;

            if (token.IsIntersectWith(italicToken))
            {
                italicToken.IsCorrect = false;
                token.IsCorrect = false;
            }

            if (italicToken.OpenIndex < token.OpenIndex && italicToken.IsOpened)
                token.IsCorrect = false;
        }
    }
}