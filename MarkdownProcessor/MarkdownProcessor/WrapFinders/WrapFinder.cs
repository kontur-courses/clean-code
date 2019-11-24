using System;
using System.Collections.Generic;
using System.Linq;
using MarkdownProcessor.TextWraps;

namespace MarkdownProcessor.WrapFinders
{
    public abstract class WrapFinder
    {
        protected WrapFinder(ITextWrapType textWrapType) => TextWrapType = textWrapType;

        protected ITextWrapType TextWrapType { get; }
        protected bool PreviousCharacterIsEscaping { get; private set; }

        public IEnumerable<TextWrap> GetWraps(string text, IReadOnlyList<TextWrap> forbiddenToIntersectingWraps = null)
        {
            if (!(forbiddenToIntersectingWraps is null) && forbiddenToIntersectingWraps.Count == 0)
                throw new ArgumentException("Passed empty sequence", nameof(forbiddenToIntersectingWraps));
            var currentForbiddenWrapIndex = forbiddenToIntersectingWraps is null ? (int?)null : 0;

            var openMarkerPositions = new Stack<int>();
            var position = 0;
            PreviousCharacterIsEscaping = false;

            while (position < text.Length)
            {
                if (position + TextWrapType.CloseWrapMarker.Length >= text.Length) break;

                if (CurrentCharacterIsEscape())
                {
                    PreviousCharacterIsEscaping = true;
                    position++;
                    continue;
                }

                if (IsCurrentPositionInForbiddenWrap(position, forbiddenToIntersectingWraps, currentForbiddenWrapIndex))
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

                if (ShouldUpdateCurrentForbiddenWrapIndex(position, forbiddenToIntersectingWraps,
                                                          currentForbiddenWrapIndex))
                    currentForbiddenWrapIndex++;

                bool CurrentCharacterIsEscape() => !PreviousCharacterIsEscaping &&
                                                   text[position] == Markdown.EscapeCharacter;
            }
        }

        protected abstract bool IsValidOpenMarker(int markerIndex, string text);
        protected abstract bool IsValidCloseMarker(int markerIndex, string text);

        protected bool IsTheMostSpecificOpenMarker(int markerIndex, string text) =>
            TextWrapType.OpenWrapMarker.Length ==
            Markdown.WrapFinderByWrapType.Keys.Where(wrapType => TextSubstringEqualsToOtherString(
                                                         markerIndex, text, wrapType.OpenWrapMarker))
                    .Max(wrapType => wrapType.OpenWrapMarker.Length);

        protected bool IsTheMostSpecificCloseMarker(int markerIndex, string text) =>
            TextWrapType.CloseWrapMarker.Length ==
            Markdown.WrapFinderByWrapType.Keys
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

        private static TextWrap? TryGetCurrentForbiddenWrap(IReadOnlyList<TextWrap> forbiddenToIntersectingWraps,
                                                            int? currentForbiddenWrapIndex) =>
            forbiddenToIntersectingWraps is null || !currentForbiddenWrapIndex.HasValue
                ? (TextWrap?)null
                : forbiddenToIntersectingWraps[currentForbiddenWrapIndex.Value];

        private static bool IsCurrentPositionInForbiddenWrap(int currentPosition,
                                                             IReadOnlyList<TextWrap> forbiddenToIntersectingWraps,
                                                             int? currentForbiddenWrapIndex)
        {
            var currentForbiddenWrap = TryGetCurrentForbiddenWrap(forbiddenToIntersectingWraps,
                                                                  currentForbiddenWrapIndex);
            if (currentForbiddenWrap is null)
                return false;

            return currentPosition >= currentForbiddenWrap.Value.OpenMarkerIndex &&
                   currentPosition <= currentForbiddenWrap.Value.CloseMarkerIndex;
        }

        private static bool ShouldUpdateCurrentForbiddenWrapIndex(int currentPosition,
                                                                  IReadOnlyList<TextWrap> forbiddenToIntersectingWraps,
                                                                  int? currentForbiddenWrapIndex)
        {
            var currentForbiddenWrap = TryGetCurrentForbiddenWrap(forbiddenToIntersectingWraps,
                                                                  currentForbiddenWrapIndex);
            if (currentForbiddenWrap is null)
                return false;

            var previousPositionWasForbiddenCloseMarker = currentPosition - 1 >= 0 &&
                                                          currentForbiddenWrap.Value.CloseMarkerIndex ==
                                                          currentPosition - 1;
            var currentForbiddenWrapIsNotLast = currentForbiddenWrapIndex + 1 < forbiddenToIntersectingWraps.Count;

            return previousPositionWasForbiddenCloseMarker && currentForbiddenWrapIsNotLast;
        }
    }
}