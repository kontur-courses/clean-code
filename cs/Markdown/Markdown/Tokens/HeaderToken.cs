using Markdown.Models;
using Markdown.Tokens.Patterns;

namespace Markdown.Tokens
{
    public class HeaderToken : IToken
    {
        public TagType TagType => TagType.Header;
        public ITokenPattern Pattern { get; } = new ParagraphTokenPattern("#");
        public TokenHtmlRepresentation TokenHtmlRepresentation { get; } = new()
        {
            OpenTag = new TagReplacer("<h1>", 2),
            CloseTag = new TagReplacer("</h1>", 1)
        };
    }
}