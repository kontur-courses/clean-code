namespace MarkdownProcessor.TextWraps
{
    public interface ITextWrap
    {
        string OpenWrapMarker { get; }
        string CloseWrapMarker { get; }
    }
}