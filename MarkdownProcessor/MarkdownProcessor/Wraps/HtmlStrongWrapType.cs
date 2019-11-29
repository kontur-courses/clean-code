namespace Markdown.Wraps
{
    public class HtmlStrongWrapType : WrapType
    {
        public override string OpenWrapMarker => "<strong>";
        public override string CloseWrapMarker => "</strong>";
    }
}