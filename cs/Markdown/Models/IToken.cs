namespace Markdown.Models
{
    public interface IToken
    {
        public TagType TagType { get; set; }
        public ITokenQuery Query { get; set; }
        public IRenderStyle RenderStyle { get; set; }
    }
}