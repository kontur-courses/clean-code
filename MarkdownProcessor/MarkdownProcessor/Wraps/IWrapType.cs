namespace Markdown.Wraps
{
    public interface IWrapType
    {
        string OpenWrapMarker { get; }
        string CloseWrapMarker { get; }
    }
}