namespace Markdown.Wraps
{
    public class DoubleUnderscoresWrapType : WrapType
    {
        public override string OpenWrapMarker => "__";
        public override string CloseWrapMarker => OpenWrapMarker;
    }
}