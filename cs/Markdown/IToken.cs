namespace Markdown
{
    public interface IToken
    {
        public int Length { get; }
        public int StartIndex { get; }
        public ITagInfo TagInfo { get; }
        public int EndTagIndex { get; }
    }
}