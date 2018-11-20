using Markdown.Md.TagHandlers;

namespace Markdown.Md.TagHandlersFactory
{
    public class EmphasisHandlerFactory : ITagHandlersFactory
    {
        public TagHandler Create()
        {
            return new EmphasisHandler();
        }
    }
}