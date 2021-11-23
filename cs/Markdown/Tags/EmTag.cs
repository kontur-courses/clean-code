namespace Markdown.Tags
{
    public class EmTag : SelectingTag
    {
        public override string OpenHtmlTag => "<em>";
        public override string CloseHtmlTag => "</em>";
        public override string OpenMdTag => "_";
        public override string CloseMdTag => OpenMdTag;
        public override bool AllowNesting => false;
    }
}
