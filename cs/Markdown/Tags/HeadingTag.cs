namespace Markdown
{
    public class HeadingTag : ITag
    {
        public string OpeningMarkup => "#";
        public string ClosingMarkup => "\n";
        public string OpeningTag => "<h1>";
        public string ClosingTag => "</h1>";
        public bool IsBrokenMarkup(string source, int start, int length)
        {
            throw new System.NotImplementedException();
        }
    }
}