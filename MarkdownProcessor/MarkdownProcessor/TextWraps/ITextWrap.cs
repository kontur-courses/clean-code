namespace MarkdownProcessor.TextWraps
{
    public interface ITextWrap
    {
        string OpenWrapMarker { get; }
        string CloseWrapMarker { get; }

        string HtmlRepresentationOfOpenMarker { get; }
        string HtmlRepresentationOfCloseMarker { get; }
    }
}