namespace MarkdownProcessor.TextWraps
{
    public class SingleUnderscoreWrapType : ITextWrapType
    {
        public string OpenWrapMarker => "_";
        public string CloseWrapMarker => OpenWrapMarker;

        public string HtmlRepresentationOfOpenMarker => "<em>";
        public string HtmlRepresentationOfCloseMarker => "</em>";
    }
}