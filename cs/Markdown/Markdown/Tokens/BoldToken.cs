using Markdown.Models;
using Markdown.Tokens.Patterns;

namespace Markdown.Tokens
{
    public class BoldToken : IToken
    {
        public TagType TagType => TagType.Bold;
        public ITokenPattern Pattern { get; } = new PairedTokenPattern("__");
        public TokenHtmlRepresentation TokenHtmlRepresentation { get; } = new()
        {
            OpenTag = new TagReplacer("<strong>", 2),
            CloseTag = new TagReplacer("</strong>", 2)
        };
    }
}