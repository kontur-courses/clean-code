namespace Markdown.Wraps
{
    public class HtmlEmphasisWrapType : WrapType
    {
        public override string OpenWrapMarker => "<em>";
        public override string CloseWrapMarker => "</em>";
    }
}