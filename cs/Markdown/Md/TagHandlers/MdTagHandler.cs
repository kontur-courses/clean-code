namespace Markdown.Md.TagHandlers
{
    public abstract class MdTagHandler
    {
        protected MdTagHandler Successor;

        public MdTagHandler SetSuccessor(MdTagHandler successor)
        {
            Successor = successor;

            return this;
        }

        public abstract MdToken Handle(string str, int position);
    }
}