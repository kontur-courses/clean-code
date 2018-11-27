namespace Markdown.Renderers
{
    public abstract class TagHandler
    {
        protected TagHandler Successor;

        public TagHandler SetSuccessor(TagHandler successor)
        {
            Successor = successor;

            return this;
        }

        public abstract string Handle(Tag tag, bool isOpeningTag);
    }
}