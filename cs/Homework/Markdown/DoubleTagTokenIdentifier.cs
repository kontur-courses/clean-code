using System;
using System.Linq;

namespace Markdown
{
    public abstract class DoubleTagTokenIdentifier : TokenIdentifier
    {
        protected string Paragraph;

        protected DoubleTagTokenIdentifier(string tag, Func<TemporaryToken, Token> tokenCreator) : base(tag, tokenCreator)
        {
        }

        protected sealed override bool IsValid(string[] paragraphs, TemporaryToken temporaryToken)
        {
            Paragraph = paragraphs[temporaryToken.ParagraphIndex];
            var startIndex = temporaryToken.StartIndex;
            var finishIndex = startIndex + temporaryToken.Length - Tag.Length;
            return !HasSpacesAfterOpenTag(startIndex)
                   && !HasSpacesBeforeCloseTag(finishIndex)
                   && !HasDigits(startIndex, finishIndex)
                   && !HasSelectionInDifferentWords(startIndex, finishIndex)
                   && IsValidWithAdditionalRestriction(temporaryToken);
        }

        protected virtual bool IsValidWithAdditionalRestriction(TemporaryToken temporaryToken) => true;

        private bool HasSpacesAfterOpenTag(int startIndex) =>
            startIndex + 1 < Paragraph.Length && char.IsWhiteSpace(Paragraph[startIndex + 1]);

        private bool HasSpacesBeforeCloseTag(int finishIndex) =>
            finishIndex - 1 >= 0 && char.IsWhiteSpace(Paragraph[finishIndex - 1]);

        private bool HasDigits(int startIndex, int finishIndex) =>
            Paragraph[startIndex..finishIndex].Any(char.IsDigit);

        private bool HasSelectionInDifferentWords(int startIndex, int finishIndex)
        {
            return Paragraph
                .Split()
                .Select(w => (index: w.IndexOf(Tag), length: w.Length))
                .Count((wordInfo => wordInfo.index > 0 && wordInfo.index < wordInfo.length - 1)) > 1;
        }
    }
}