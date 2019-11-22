namespace MarkdownProcessor.TextWraps
{
    public struct UnderscoreWrap : ITextWrap
    {
        public string OpenWrapMarker => "_";
        public string CloseWrapMarker => OpenWrapMarker;
    }
}