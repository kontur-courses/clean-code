namespace Markdown.Tag
{
    public interface ITag
    {
        string Symbol { get; set; }
        int OpenIndex { get; set; }
        int CloseIndex { get; set; }
        string Html { get; set; }
        int Length { get; set; }
        string Content { get; set; }
        MdType Type { get; set; }
    }
}