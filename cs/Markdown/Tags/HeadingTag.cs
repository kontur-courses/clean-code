namespace Markdown
{
    public class HeadingTag : ITag
    {
        public string Opening => "\\<h1>";
        public string Closing => "\\</h1>";
        public int Priority => 3;
    }
}