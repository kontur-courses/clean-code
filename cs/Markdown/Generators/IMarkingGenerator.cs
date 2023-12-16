using Markdown.Tokens;

namespace Markdown.Generators
{
    public interface IMarkingGenerator
    {
        string Generate(IEnumerable<IToken> tokens);
    }
}
