using System;
using System.Linq;
using Markdown.Parser;
using Markdown.Tokens;

namespace Markdown.TokenIdentifiers
{
    public abstract class DoubleTagTokenIdentifier<TToken> : TokenIdentifier<TToken>
    where TToken : Token
    {
        protected string Paragraph;

        protected DoubleTagTokenIdentifier(IParser<TToken> parser, string selector) : base(parser, selector)
        {
        }

        protected sealed override bool IsValid(string[] paragraphs, TemporaryToken temporaryToken)
        {
            Paragraph = paragraphs[temporaryToken.ParagraphIndex];
            var startIndex = temporaryToken.StartIndex;
            var finishIndex = startIndex + temporaryToken.Length - Selector.Length;
            return !HasSpacesAfterOpenTag(startIndex)
                   && !HasSpacesBeforeCloseTag(finishIndex)
                   && !HasDigits(startIndex, finishIndex)
                   && !HasSelectionInDifferentWords()
                   && IsValidWithAdditionalRestriction(temporaryToken);
        }

        protected virtual bool IsValidWithAdditionalRestriction(TemporaryToken temporaryToken) => true;

        private bool HasSpacesAfterOpenTag(int startIndex) =>
            startIndex + 1 < Paragraph.Length && char.IsWhiteSpace(Paragraph[startIndex + 1]);

        private bool HasSpacesBeforeCloseTag(int finishIndex) =>
            finishIndex - 1 >= 0 && char.IsWhiteSpace(Paragraph[finishIndex - 1]);

        private bool HasDigits(int startIndex, int finishIndex) =>
            Paragraph[startIndex..finishIndex].Any(char.IsDigit);

        private bool HasSelectionInDifferentWords()
        {
            return Paragraph
                .Split()
                .Select(w => (index: w.IndexOf(Selector), length: w.Length))
                .Count((wordInfo => wordInfo.index > 0 && wordInfo.index < wordInfo.length - 1)) > 1;
        }
    }
}