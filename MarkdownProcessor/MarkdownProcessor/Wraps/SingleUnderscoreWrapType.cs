namespace MarkdownProcessor.Wraps
{
    public class SingleUnderscoreWrapType : WrapType
    {
        public override string OpenWrapMarker => "_";
        public override string CloseWrapMarker => OpenWrapMarker;
    }
}