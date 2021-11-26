using System;
using Markdown.Tokens;

namespace Markdown.TokenIdentifiers
{
    public class ItalicTokenIdentifier : DoubleTagTokenIdentifier
    {
        public ItalicTokenIdentifier(string tag, Func<TemporaryToken, Token> tokenCreator) : base(tag, tokenCreator)
        {
        }

        protected override bool IsValidWithAdditionalRestriction(TemporaryToken temporaryToken)
        {
            return !HasNestedBoldTags(temporaryToken)
                   && !IntersectedWithBoldTags(temporaryToken)
                   && !HasUnderlineAfterCloseTag(temporaryToken);
        }

        private bool HasNestedBoldTags(TemporaryToken temporaryToken)
        {
            var firstOccurrence = temporaryToken.Value.IndexOf("__");
            if (firstOccurrence == -1)
                return false;
            var secondOccurence = temporaryToken.Value.LastIndexOf("__");
            return firstOccurrence != secondOccurence;
        }

        private bool IntersectedWithBoldTags(TemporaryToken temporaryToken)
        {
            var innerOccurence = temporaryToken.Value.Contains("__");
            if (!innerOccurence)
                return false;
            return Paragraph.IndexOf("__") < temporaryToken.StartIndex
                   || Paragraph.LastIndexOf("__") > temporaryToken.StartIndex + temporaryToken.Length;
        }

        private bool HasUnderlineAfterCloseTag(TemporaryToken temporaryToken)
        {
            var finishIndex = temporaryToken.StartIndex + temporaryToken.Length;
            return finishIndex + 1 < Paragraph.Length
                   && Paragraph[finishIndex] == '_';
        }
    }
}