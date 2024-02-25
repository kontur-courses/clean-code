namespace Markdown.Tokens;

public static class TokensGenerators
{
    public static readonly IReadOnlyDictionary<string, Func<int, Token>> Generators =
        new Dictionary<string, Func<int, Token>>()
        {
            {
                new ItalicsToken(0, 0).Separator, 
                (int openIndex) => new ItalicsToken(openIndex)
            },
            {
                new BoldToken(0, 0).Separator,
                (int openIndex) => new BoldToken(openIndex)
            },
            {
                new ParagraphToken(0, 0).Separator, 
                (int openIndex) => new ParagraphToken(openIndex)
            },
            {
                new ScreeningToken(0, 0).Separator,
                (int openIndex) => new ScreeningToken(openIndex, openIndex)
            },
            {
                new ListItemToken(0, 0).Separator, 
                (int openIndex) => new ListItemToken(openIndex)
            },
            {
                new MarkedListToken(0, 0).Separator, 
                (int openIndex) => new MarkedListToken(openIndex)
            }
        };
}