namespace Markdown.Enums
{
    /// <summary>
    /// <para>HeaderMarker = #</para>
    /// <para>BoldMarker = __</para>
    /// <para>ItalicsMarker = _</para>
    /// </summary>
    public enum TokenType
    {
        Text,
        Header,
        BoldStart,
        BoldEnd,
        ItalicsStart,
        ItalicsEnd,
        Newline,
        UrlTitleDelimiter,
        Url,
        UrlTitle,
        UrlEnd,
        UrlStart,
        LinkEnd,
        LinkText,
        LinkStart
    }
}
