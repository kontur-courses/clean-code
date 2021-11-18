namespace Markdown
{
    public class ItalicTag : ITag
    {
        public string Opening => "\\<em>";
        public string Closing => "\\</em>";
        public int Priority => 1;
    }
}