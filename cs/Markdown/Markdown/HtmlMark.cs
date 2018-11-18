namespace Markdown
{
    public class HtmlMark:Mark
    {
        public HtmlMark(string sign, string openingTag):base(sign, openingTag, openingTag.Insert(1, "/")) { }
    }
}
