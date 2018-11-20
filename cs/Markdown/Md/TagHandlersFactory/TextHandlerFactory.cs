using Markdown.Md.TagHandlers;

namespace Markdown.Md.TagHandlersFactory
{
    public class TextHandlerFactory : ITagHandlersFactory
    {
        public TagHandler Create()
        {
            return new TextHandler();
        }
    }
}