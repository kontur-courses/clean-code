using System;
using Markdown.Parser;
using Markdown.Tokens;

namespace Markdown.TokenIdentifiers
{
    public class StrongTokenIdentifier : DoubleTagTokenIdentifier<MarkdownToken>
    {
        public StrongTokenIdentifier(IParser<MarkdownToken> parser, string selector) : base(parser, selector)
        {
        }

        public override StrongToken CreateToken(TemporaryToken temporaryToken)
        {
            var innerValue = temporaryToken.Value[Selector.Length..^Selector.Length];
            return new StrongToken(temporaryToken.Value, temporaryToken.Selector, temporaryToken.ParagraphIndex,
                temporaryToken.StartIndex) {SubTokens = Parser.Parse(innerValue)};
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
            var startIndex = Selector.Length;
            var finishIndex = token.Length - Selector.Length;
            return token.Value[startIndex..finishIndex];
        }
    }
}