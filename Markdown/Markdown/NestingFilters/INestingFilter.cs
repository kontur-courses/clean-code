using Markdown.Tokens;

namespace Markdown.NestingFilters;

public interface INestingFilter
{
    void Filter(Token token);
}