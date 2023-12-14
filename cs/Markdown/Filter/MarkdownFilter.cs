using Markdown.Tokens;

namespace Markdown.Filter;

//Реализовать по принципу FluentAPI
public class MarkdownFilter : ITokenFilter
{
    public List<Token> FilterTokens(List<Token> tokens, string line)
    {
        foreach (var token in tokens)
        {
        }

        return tokens;
    }
}