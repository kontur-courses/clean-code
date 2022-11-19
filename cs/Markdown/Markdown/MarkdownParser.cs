namespace Markdown
{
    public class MarkdownParser : IParser<MdTag>
    {
        public Dictionary<MdTag, (int startTagIndex, int closeTagIndex)> GetIndexesTags(string text)
        {
            return new Dictionary<MdTag, (int, int)>();
        }
    }
}