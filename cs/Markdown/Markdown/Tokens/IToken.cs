using Markdown.Models;
using Markdown.Tokens.Patterns;

namespace Markdown.Tokens
{
    public interface IToken
    {
        TagType TagType { get; }
        ITokenPattern Pattern { get; }
        TokenHtmlRepresentation TokenHtmlRepresentation { get; }
    }
}