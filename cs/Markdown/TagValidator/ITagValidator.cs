using Markdown.Tags;

namespace Markdown.TagValidator
{
    public interface ITagValidator
    {
        bool IsValid(string text, ITag tag, SubTagOrder order, int start);
    }
}