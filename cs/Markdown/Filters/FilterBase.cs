using Markdown.Tokens;

namespace Markdown.Filters;

public abstract class FilterBase
{
    protected readonly FilterBase? nextFilter;

    public FilterBase(FilterBase? nextFilter)
    {
        this.nextFilter = nextFilter;
    }

    public abstract IList<IToken> Filter(IList<IToken> tokens);
}
