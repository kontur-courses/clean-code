using Markdown.Tokens;

namespace Markdown.Models
{
    public interface IToken
    {
        TagType TagType { get; }
        ITokenPattern Pattern { get; }
        TokenHtmlRepresentation TokenHtmlRepresentation { get; }
    }
}