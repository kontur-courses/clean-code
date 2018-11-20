using Markdown.Md.TagHandlers;

namespace Markdown.Md.TagHandlersFactory
{
    public class StrongEmphasisHandlerFactory : ITagHandlersFactory
    {
        public TagHandler Create()
        {
            return new StrongEmphasisHandler();
        }
    }
}