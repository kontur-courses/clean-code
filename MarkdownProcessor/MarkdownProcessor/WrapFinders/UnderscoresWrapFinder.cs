using MarkdownProcessor.TextWraps;

namespace MarkdownProcessor.WrapFinders
{
    public class UnderscoresWrapFinder : WrapFinder
    {
        public UnderscoresWrapFinder(ITextWrapType textWrapType) : base(textWrapType) { }

        public UnderscoresWrapFinder(ITextWrapType textWrapType, TextWrap[] wrapBorders) : base(
            textWrapType, wrapBorders) { }

        protected override bool IsValidOpenMarker(int markerIndex, string text) =>
            !IsEscapedCharacter && EqualsToOpenMarker(markerIndex, text) &&
            (markerIndex == 0 || HasWhitespaceBefore(markerIndex, text)) &&
            !HasWhitespaceAfter(markerIndex, text, TextWrapType.OpenWrapMarker.Length);

        protected override bool IsValidCloseMarker(int markerIndex, string text) =>
            !IsEscapedCharacter && EqualsToCloseMarker(markerIndex, text) &&
            !HasWhitespaceBefore(markerIndex, text) &&
            (markerIndex + TextWrapType.CloseWrapMarker.Length == text.Length ||
             HasWhitespaceAfter(markerIndex, text, TextWrapType.CloseWrapMarker.Length));
    }
}