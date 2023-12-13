using Markdown.Tokens;

namespace Markdown.Generators
{
    public interface IGenerator
    {
        string Generate(IEnumerable<IToken> tokens);
    }
}
