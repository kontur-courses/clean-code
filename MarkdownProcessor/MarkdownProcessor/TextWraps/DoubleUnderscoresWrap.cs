namespace MarkdownProcessor.TextWraps
{
    public struct DoubleUnderscoresWrap : ITextWrap
    {
        public string OpenWrapMarker => "__";
        public string CloseWrapMarker => OpenWrapMarker;

        public string HtmlRepresentationOfOpenMarker => "<strong>";
        public string HtmlRepresentationOfCloseMarker => "</strong>";
    }
}