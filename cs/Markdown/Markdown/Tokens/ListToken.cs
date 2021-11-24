using Markdown.Models;
using Markdown.Tokens.Patterns;

namespace Markdown.Tokens
{
    public class ListToken : IToken
    {
        public TagType TagType => TagType.UnorderedList;
        public ITokenPattern Pattern { get; } = new ParagraphTokenPattern("-");
        public TokenHtmlRepresentation TokenHtmlRepresentation { get; } = new()
        {
            OpenTag = new TagReplacer("<li>", 2),
            CloseTag = new TagReplacer("</li>", 1)
        };
    }
}