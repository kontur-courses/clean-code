using Markdown.Tag;
using Markdown.Token;

namespace Markdown.Syntax;

public interface ITokenToMarkupSyntax
{
    ITag ConvertTag(IToken token);
    IList<string> GetSupportedTagParameters(string tagSeparator);
}