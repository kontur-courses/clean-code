namespace Markdown
{
    public interface ITagInfo
    {
        public string OpenTagInMd { get; }
        public string TagForConverting { get; }
    }
}