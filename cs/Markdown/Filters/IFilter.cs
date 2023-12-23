using Markdown.Tokens;

namespace Markdown.Filters;

public interface IFilter
{
    int Order { get; }
    IList<IToken> Filter(IList<IToken> tokens);
}
