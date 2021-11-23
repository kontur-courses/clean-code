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

        internal override bool Validate(IMdParser parser)
        {
            this.ValidatePlacedCorrectly(parser.TextToParse);
            ValidateInteractions(parser.Tokens);

            return IsCorrect;
        }

        private void ValidateInteractions(IReadOnlyDictionary<string, Token> tokens)
        {
            if (!IsCorrect || !tokens.TryGetValue(ItalicToken.Separator, out var italicToken))
                return;

            if (this.IsIntersectWith(italicToken))
            {
                italicToken.IsCorrect = false;
                IsCorrect = false;
            }

            if (italicToken.OpenIndex < OpenIndex && italicToken.IsOpened)
                IsCorrect = false;
        }
    }
}