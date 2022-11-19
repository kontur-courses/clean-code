namespace Markdown
{
    public class Replacer<T> where T: ITag
    {
        private readonly
            Dictionary<MdTag, HtmlTag> _markdownToHtml = new Dictionary<MdTag, HtmlTag>();
        
        public string ReplaceTagOnHtml(Dictionary<T, (int startTagIndex, int closeTagIndex)> tags, string text)
        {
            return String.Empty;
        }
    }
}
