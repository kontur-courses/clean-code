namespace Markdown
{
    public class HeadingTag : Tag
    {
        public override string OpenHTMLTag => "<h1>";
        public override string CloseHTMLTag => "</h1>";
        public override string OpenMdTag => "# ";
        public override string CloseMdTag => "\n";
        public override bool AllowNesting => true;
    }
}
