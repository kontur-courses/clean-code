namespace Markdown.Md.PairedTagsHandlers
{
    public abstract class PairedTagsHandler
    {
        protected PairedTagsHandler Successor;

        public PairedTagsHandler SetSuccessor(PairedTagsHandler successor)
        {
            Successor = successor;

            return this;
        }

        public abstract void Handle(
            MdPairedTagsState state,
            int position,
            MdToken token);
    }
}