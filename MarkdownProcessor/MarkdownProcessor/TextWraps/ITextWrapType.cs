namespace MarkdownProcessor.TextWraps
{
    public interface ITextWrapType // TODO: union TextWrap and WrapBorder
    {
        string OpenWrapMarker { get; }
        string CloseWrapMarker { get; }

        string HtmlRepresentationOfOpenMarker { get; }
        string HtmlRepresentationOfCloseMarker { get; }
    }
}