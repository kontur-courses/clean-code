namespace Markdown
{
    public class ItalicsTag : ITag
    {
        public string OpeningMarkup => "_";
        public string ClosingMarkup => "_";
        public string OpeningTag => "<em>";
        public string ClosingTag => "</em>";
    }
}