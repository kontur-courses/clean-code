namespace Markdown
{
    public class ShieldingTag : ITag
    {
        public string OpeningMarkup => "\\";
        public string ClosingMarkup => "";
        public string OpeningTag => "";
        public string ClosingTag => "";
        public bool IsBrokenMarkup(string source, int start, int length)
        {
            return false;
        }
    }
}