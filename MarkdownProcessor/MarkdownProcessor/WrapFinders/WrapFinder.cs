using System;
using System.Collections.Generic;
using System.Linq;
using MarkdownProcessor.TextWraps;

namespace MarkdownProcessor.WrapFinders
{
    public abstract class WrapFinder
    {
        protected WrapFinder(ITextWrapType textWrapType) => TextWrapType = textWrapType;

        protected WrapFinder(ITextWrapType textWrapType, IReadOnlyList<TextWrap> forbiddenToIntersectingWraps) :
            this(textWrapType)
        {
            if (forbiddenToIntersectingWraps.Count == 0)
                throw new ArgumentException("Passed empty sequence", nameof(forbiddenToIntersectingWraps));

            this.forbiddenToIntersectingWraps = forbiddenToIntersectingWraps;
            currentForbiddenWrapIndex = 0;
        }

        protected ITextWrapType TextWrapType { get; }
        protected bool PreviousCharacterIsEscaping { get; private set; }

        private readonly IReadOnlyList<TextWrap> forbiddenToIntersectingWraps;
        private int? currentForbiddenWrapIndex;

        private TextWrap? CurrentForbiddenWrap =>
            forbiddenToIntersectingWraps is null || !currentForbiddenWrapIndex.HasValue
                ? (TextWrap?)null
                : forbiddenToIntersectingWraps[currentForbiddenWrapIndex.Value];

        public IEnumerable<TextWrap> GetPairsOfMarkers(string text)
        {
            if (currentForbiddenWrapIndex.HasValue) currentForbiddenWrapIndex = 0;
            var openMarkerPositions = new Stack<int>();
            var position = 0;
            PreviousCharacterIsEscaping = false;

            while (position < text.Length)
            {
                if (position + TextWrapType.CloseWrapMarker.Length >= text.Length) break;

                if (IsCurrentCharacterEscape())
                {
                    PreviousCharacterIsEscaping = true;
                    position++;
                    continue;
                }

                if (IsCurrentPositionInForbiddenWrap(position))
                {
                    position++;
                    continue;
                }

                if (IsValidOpenMarker(position, text))
                    openMarkerPositions.Push(position);
                else if (IsValidCloseMarker(position, text) && openMarkerPositions.Count > 0)
                    yield return new TextWrap(TextWrapType, openMarkerPositions.Pop(), position);

                position++;
                PreviousCharacterIsEscaping = false;
                UpdateCurrentForbiddenWrapIndex(position);

                bool IsCurrentCharacterEscape() => !PreviousCharacterIsEscaping &&
                                                   text[position] == Markdown.EscapeCharacter;
            }
        }

        protected abstract bool IsValidOpenMarker(int markerIndex, string text);

        /*
         TODO: Вопрос! Специально протаскиваю везде аргументы (int markerIndex, string text), чтобы не делать их
         такими же, как "protected bool PreviousCharacterIsEscaping { get; private set; }"
         
         Не хочется, чтобы у какого-либо метода в данном классе, кроме GetPairsOfMarkers(), была возможность изменять
         эти свойства.
         Но приходится часто их копипастить. Чем жертвовать - инкапсуляцией или копипастом?
         */
        protected abstract bool IsValidCloseMarker(int markerIndex, string text);

        protected bool IsTheMostSpecificOpenMarker(int markerIndex, string text) =>
            TextWrapType.OpenWrapMarker.Length ==
            Markdown.WrapTypes.Where(wrapType => TextSubstringEqualsToOtherString(markerIndex, text,
                                                                                  wrapType.OpenWrapMarker))
                    .Max(wrapType => wrapType.OpenWrapMarker.Length);

        protected bool IsTheMostSpecificCloseMarker(int markerIndex, string text) =>
            TextWrapType.CloseWrapMarker.Length ==
            Markdown.WrapTypes
                    .Where(wrapType => TextSubstringEqualsToOtherString(
                               markerIndex + TextWrapType.CloseWrapMarker.Length - wrapType.CloseWrapMarker.Length,
                               text, wrapType.CloseWrapMarker))
                    .Max(wrapType => wrapType.CloseWrapMarker.Length);

        protected static bool HasWhitespaceBefore(int markerIndex, string text) =>
            markerIndex - 1 >= 0 && Markdown.WhiteSpaceSymbols.Contains(text[markerIndex - 1]);

        protected static bool HasWhitespaceAfter(int markerIndex, string text, int markerLength) =>
            markerIndex + markerLength < text.Length &&
            Markdown.WhiteSpaceSymbols.Contains(text[markerIndex + markerLength]);

        protected bool EqualsToOpenMarker(int markerIndex, string text) =>
            TextSubstringEqualsToOtherString(markerIndex, text, TextWrapType.OpenWrapMarker);

        protected bool EqualsToCloseMarker(int markerIndex, string text) =>
            TextSubstringEqualsToOtherString(markerIndex, text, TextWrapType.CloseWrapMarker);

        private static bool TextSubstringEqualsToOtherString(int startIndex, string text, string otherString) =>
            startIndex + otherString.Length < text.Length &&
            text.Substring(startIndex, otherString.Length) == otherString;

        private bool IsCurrentPositionInForbiddenWrap(int currentPosition)
        {
            if (!CurrentForbiddenWrap.HasValue) return false;

            return currentPosition >= CurrentForbiddenWrap?.OpenMarkerIndex &&
                   currentPosition <= CurrentForbiddenWrap?.CloseMarkerIndex;
        }

        private void UpdateCurrentForbiddenWrapIndex(int currentPosition)
        {
            if (!CurrentForbiddenWrap.HasValue) return;

            var previousPositionWasCloseMarker = currentPosition - 1 >= 0 &&
                                                 CurrentForbiddenWrap?.CloseMarkerIndex == currentPosition - 1;
            var currentWrapIsNotLast = currentForbiddenWrapIndex + 1 < forbiddenToIntersectingWraps.Count;

            if (previousPositionWasCloseMarker && currentWrapIsNotLast)
                currentForbiddenWrapIndex++;
        }
    }
}