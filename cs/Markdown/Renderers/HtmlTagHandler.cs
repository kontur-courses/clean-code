namespace Markdown.Renderers
{
    public abstract class HtmlTagHandler
    {
        protected HtmlTagHandler Successor;

        public HtmlTagHandler SetSuccessor(HtmlTagHandler successor)
        {
            Successor = successor;

            return this;
        }

        public abstract string Handle(ITokenNode tokenNode);
    }
}