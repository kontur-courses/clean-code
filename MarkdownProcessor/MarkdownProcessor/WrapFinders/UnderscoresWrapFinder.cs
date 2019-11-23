using MarkdownProcessor.TextWraps;

namespace MarkdownProcessor.WrapFinders
{
    public class UnderscoresWrapFinder : WrapFinder
    {
        public UnderscoresWrapFinder(ITextWrap textWrap) : base(textWrap) { }
        public UnderscoresWrapFinder(ITextWrap textWrap, WrapBorder[] wrapBorders) : base(textWrap, wrapBorders) { }

        protected override bool IsValidOpenMarker(int markerIndex, string text) =>
            !IsEscapedCharacter && EqualsToOpenMarker(markerIndex, text) &&
            (markerIndex == 0 || HasWhitespaceBefore(markerIndex, text)) &&
            !HasWhitespaceAfter(markerIndex, text, TextWrap.OpenWrapMarker.Length);

        protected override bool IsValidCloseMarker(int markerIndex, string text) =>
            !IsEscapedCharacter && EqualsToCloseMarker(markerIndex, text) &&
            !HasWhitespaceBefore(markerIndex, text) &&
            (markerIndex + TextWrap.CloseWrapMarker.Length == text.Length ||
             HasWhitespaceAfter(markerIndex, text, TextWrap.CloseWrapMarker.Length));
    }
}