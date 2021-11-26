using System;
using Markdown.Tokens;

namespace Markdown.TokenIdentifiers
{
    public class BoldTokenIdentifier : DoubleTagTokenIdentifier
    {
        public BoldTokenIdentifier(string tag, Func<TemporaryToken, Token> tokenCreator) : base(tag, tokenCreator)
        {
        }

        protected override bool IsValidWithAdditionalRestriction(TemporaryToken temporaryToken)
        {
            return !IntersectedWithItalicTags(temporaryToken)
                   && !InsideItalicTags(temporaryToken);
        }

        private bool IntersectedWithItalicTags(TemporaryToken temporaryToken)
        {
            var valueWithoutTags = GetValueWithoutTags(temporaryToken);
            var innerOccurrence = valueWithoutTags.Contains("_");
            if (!innerOccurrence)
                return false;
            return Paragraph.IndexOf("_") < temporaryToken.StartIndex
                   || Paragraph.LastIndexOf("_") > temporaryToken.StartIndex + temporaryToken.Length;
        }

        private bool InsideItalicTags(TemporaryToken token)
        {
            var leftOccurrence = Paragraph.IndexOf('_');
            var rightOccurrence = Paragraph.LastIndexOf('_');
            return leftOccurrence != rightOccurrence
                   && leftOccurrence < token.StartIndex
                   && rightOccurrence > token.StartIndex + token.Length;
        }

        private string GetValueWithoutTags(TemporaryToken token)
        {
            var startIndex = Tag.Length;
            var finishIndex = token.Length - Tag.Length;
            return token.Value[startIndex..finishIndex];
        }
    }
}