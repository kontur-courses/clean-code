namespace Markdown
{
    public class EmTag : SelectingTag
    {
        public override string OpenHTMLTag => "<em>";
        public override string CloseHTMLTag => "</em>";
        public override string OpenMdTag => "_";
        public override string CloseMdTag => OpenMdTag;
        public override bool AllowNesting => false;
    }
}
