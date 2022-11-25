
using Markdown.TagValidator;

namespace Markdown.Tags
{
    public interface ITag
    {
        TagType Type { get; }
        string OpeningSubTag { get; }
        string ClosingSubTag { get; }
        ITagValidator Validator { get; }
        string GetSubTag(SubTagOrder order);
        bool IsValid(string text, SubTagOrder order, int start);
    }
}
