namespace Markdown
{
    
    public interface IParser<T> where T: ITag
    {
        public Dictionary<T, (int startTagIndex, int closeTagIndex)> GetIndexesTags(string text);
    }
}
