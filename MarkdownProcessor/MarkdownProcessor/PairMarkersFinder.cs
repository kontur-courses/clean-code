using System.Collections.Generic;
using MarkdownProcessor.TextWraps;

namespace MarkdownProcessor
{
    public abstract class PairMarkersFinder
    {
        private (int openMarkerIndex, int closeMarkerIndex)[] forbiddenToIntersectingWraps; // TODO: implement this

        public PairMarkersFinder(ITextWrap textWrap) => TextWrap = textWrap;

        public PairMarkersFinder(ITextWrap textWrap, (int openMarkerIndex, int closeMarkerIndex)[]
                                     forbiddenToIntersectingWraps) : this(textWrap) =>
            this.forbiddenToIntersectingWraps = forbiddenToIntersectingWraps;

        public ITextWrap TextWrap { get; }

        protected bool IsEscapedCharacter { get; private set; }

        private static Stack<int> OpenMarkerPositions => new Stack<int>();

        public IEnumerable<(int openMarkerIndex, int closeMarkerIndex)> GetPairsOfMarkers(string text)
        {
            var position = 0;
            IsEscapedCharacter = false;

            while (position < text.Length)
            {
                if (position + TextWrap.CloseWrapMarker.Length >= text.Length) break;

                if (text[position] == Markdown.EscapeCharacter)
                {
                    IsEscapedCharacter = true;
                    position++;
                    continue;
                }

                if (IsValidOpenMarker(position, text))
                {
                    OpenMarkerPositions.Push(position);

                    position += TextWrap.OpenWrapMarker.Length;
                    continue;
                }

                if (IsValidCloseMarker(position, text))
                {
                    yield return (OpenMarkerPositions.Pop(), position);

                    position += TextWrap.CloseWrapMarker.Length;
                    continue;
                }

                IsEscapedCharacter = false;

                position++;
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
    }
}