using Markdown.Models;

namespace Markdown.Tokens
{
    public class ItalicToken : IToken
    {
        public TagType TagType { get; set; } = TagType.Italic;
        public ITokenQuery Query { get; set; } = new ItalicQuery();
        public IRenderStyle RenderStyle { get; set; } = new ItalicRenderStyle();
    }
}