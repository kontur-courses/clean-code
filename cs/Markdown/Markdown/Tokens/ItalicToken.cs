using System.Collections.Generic;
using Markdown.Models;
using Markdown.Tokens.Patterns;

namespace Markdown.Tokens
{
    public class ItalicToken : IToken
    {
        public TagType TagType => TagType.Italic;
        public ITokenPattern Pattern { get; } = new PairedTokenPattern("_")
            {ForbiddenChildren = new List<TagType> {TagType.Bold}};
        public TokenHtmlRepresentation TokenHtmlRepresentation { get; } = new()
        {
            OpenTag = new TagReplacer("<em>", 1),
            CloseTag = new TagReplacer("</em>", 1)
        };
    }
}