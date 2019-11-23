namespace MarkdownProcessor.TextWraps
{
    public struct TextWrap
    {
        public TextWrap(ITextWrapType wrapType, int openMarkerIndex, int closeMarkerIndex)
        {
            WrapType = wrapType;
            OpenMarkerIndex = openMarkerIndex;
            CloseMarkerIndex = closeMarkerIndex;
        }

        public ITextWrapType WrapType { get; }
        public int OpenMarkerIndex { get; }
        public int CloseMarkerIndex { get; }
    }
}