namespace Markdown
{
    public interface IRenderer<TTag>
    {
        public IToken<TTag>[] Render(IRules rules);
    }
}
