using Markdown.DTOs;

namespace Markdown.Tags
{
    public interface ITag
    {
        string StringTag { get; }
        int Length { get; }
        TagTypeEnum TagType { get; }
        TagClassEnum TagClass { get; set; }
        ITag Parent { get; set; }
    }
}