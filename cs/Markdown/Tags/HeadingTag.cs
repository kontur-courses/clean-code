namespace Markdown.Tags
{
    public class HeadingTag : Tag
    {
        public override string OpenHtmlTag => "<h1>";
        public override string CloseHtmlTag => "</h1>";
        public override string OpenMdTag => "# ";
        public override string CloseMdTag => "\n";
        public override bool AllowNesting => true;
        public override bool IsCorrectOpenTag(string mdText, int position) => true;
        public override bool IsCorrectCloseTag(string mdText, int position) => true;
    }
}
