namespace MarkdownProcessor.TextWraps
{
    public struct DoubleUnderscoresWrapType : ITextWrapType
    {
        public string OpenWrapMarker => "__";
        public string CloseWrapMarker => OpenWrapMarker;

        public string HtmlRepresentationOfOpenMarker => "<strong>";
        public string HtmlRepresentationOfCloseMarker => "</strong>";
    }
}