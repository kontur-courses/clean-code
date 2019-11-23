using System.Collections.Generic;
using MarkdownProcessor.TextWraps;

namespace MarkdownProcessor.WrapFinders
{
    public abstract class WrapFinder
    {
        protected WrapFinder(ITextWrap textWrap) => TextWrap = textWrap;

        protected WrapFinder(ITextWrap textWrap, WrapBorder[] forbiddenToIntersectingWraps) : this(textWrap)
        {
            this.forbiddenToIntersectingWraps = forbiddenToIntersectingWraps;
            currentForbiddenWrapIndex = 0;
        }

        protected ITextWrap TextWrap { get; }

        protected bool IsEscapedCharacter { get; private set; }

        private readonly WrapBorder[] forbiddenToIntersectingWraps;
        private int? currentForbiddenWrapIndex;

        private WrapBorder? CurrentForbiddenWrap =>
            forbiddenToIntersectingWraps is null || currentForbiddenWrapIndex is null
                ? (WrapBorder?)null
                : forbiddenToIntersectingWraps[currentForbiddenWrapIndex.Value];

        private static readonly Stack<int> openMarkerPositions = new Stack<int>();

        public IEnumerable<WrapBorder> GetPairsOfMarkers(string text) // TODO: decomposition?
        {
            var position = 0;
            IsEscapedCharacter = false;

            while (position < text.Length)
            {
                if (position + TextWrap.CloseWrapMarker.Length >= text.Length) break;

                if (!IsEscapedCharacter && text[position] == Markdown.EscapeCharacter)
                {
                    IsEscapedCharacter = true;
                    position++;
                    continue;
                }

                IsEscapedCharacter = false;

                if (IsCurrentPositionInForbiddenWrap(position))
                {
                    position++;
                    continue;
                }

                if (IsValidOpenMarker(position, text))
                    openMarkerPositions.Push(position);
                else if (IsValidCloseMarker(position, text))
                    yield return new WrapBorder(openMarkerPositions.Pop(), position);

                position++;
                UpdateCurrentForbiddenWrapIndex(position);
            }
        }

        protected abstract bool IsValidOpenMarker(int markerIndex, string text);
        protected abstract bool IsValidCloseMarker(int markerIndex, string text);

        protected static bool HasWhitespaceBefore(int markerIndex, string text) =>
            markerIndex - 1 >= 0 && Markdown.WhiteSpaceSymbols.Contains(text[markerIndex - 1]);

        protected static bool HasWhitespaceAfter(int markerIndex, string text, int markerLength) =>
            markerIndex + markerLength < text.Length &&
            Markdown.WhiteSpaceSymbols.Contains(text[markerIndex + markerLength]);

        protected bool EqualsToOpenMarker(int markerIndex, string text) =>
            EqualsToOtherString(markerIndex, text, TextWrap.OpenWrapMarker);

        protected bool EqualsToCloseMarker(int markerIndex, string text) =>
            EqualsToOtherString(markerIndex, text, TextWrap.CloseWrapMarker);

        private static bool EqualsToOtherString(int markerIndex, string text, string otherString) =>
            markerIndex + otherString.Length < text.Length &&
            text.Substring(markerIndex, otherString.Length) == otherString;

        private bool IsCurrentPositionInForbiddenWrap(int currentPosition)
        {
            if (!CurrentForbiddenWrap.HasValue) return false;

            return currentPosition >= CurrentForbiddenWrap?.OpenMarkerIndex &&
                   currentPosition <= CurrentForbiddenWrap?.OpenMarkerIndex;
        }

        private void UpdateCurrentForbiddenWrapIndex(int currentPosition)
        {
            if (!CurrentForbiddenWrap.HasValue) return;

            var previousPositionWasCloseMarker = currentPosition - 1 >= 0 &&
                                                 CurrentForbiddenWrap?.CloseMarkerIndex == currentPosition - 1;
            var currentWrapIsNotLast = currentForbiddenWrapIndex + 1 < forbiddenToIntersectingWraps.Length;

            if (previousPositionWasCloseMarker && currentWrapIsNotLast)
                currentForbiddenWrapIndex++;
        }
    }
}