namespace Markdown.Tags
{
    public class StrongTag : SelectingTag
    {
        public override string OpenHtmlTag => "<strong>";
        public override string CloseHtmlTag => "</strong>";
        public override string OpenMdTag => "__";
        public override string CloseMdTag => OpenMdTag;
        public override bool AllowNesting => true;
    }
}
