using System.Collections.Generic;

namespace Markdown.Md.TagHandlers
{
    public abstract class TagHandler
    {
        protected TagHandler Successor;

        public TagHandler SetSuccessor(TagHandler successor)
        {
            Successor = successor;

            return this;
        }

        public abstract TokenNode Handle(string str, int position, IReadOnlyCollection<ITokenNode> openingTokenNodes);
    }
}