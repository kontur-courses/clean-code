namespace MarkdownProcessor.WrapFinders
{
    public struct WrapBorder
    {
        public WrapBorder(int openMarkerIndex, int closeMarkerIndex)
        {
            OpenMarkerIndex = openMarkerIndex;
            CloseMarkerIndex = closeMarkerIndex;
        }

        public int OpenMarkerIndex { get; }
        public int CloseMarkerIndex { get; }
    }
}