namespace Markdown.ASTNodes
{
    public interface IElement
    {
        IElement Child { get; set; }
    }
}
