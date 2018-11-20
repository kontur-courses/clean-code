using Markdown.Md.TagHandlers;

namespace Markdown
{
    public interface ITagHandlersFactory
    {
        TagHandler Create();
    }
}