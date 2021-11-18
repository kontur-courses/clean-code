namespace Markdown
{
    public class BoldTag : ITag
    {
        public string Opening => "\\<strong>";
        public string Closing => "\\</strong>";
        public int Priority => 2;
    }
}