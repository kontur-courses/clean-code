namespace Markdown
{
    public class HeadingTag : ITag
    {
        public string OpeningTag => "\\<h1>";
        public string ClosingTag => "\\</h1>";
        public string OpeningMarkup => "#";
        public string ClosingMarkup => "\n";
    }
}