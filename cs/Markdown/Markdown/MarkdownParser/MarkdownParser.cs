namespace Markdown;

public class MarkdownParser(List<TokenParser> parsers) : IMarkdownParser
{
    private readonly List<TokenParser> parsers = parsers;

    public List<Token> ParseTextToTokens(string text)
    {
        var tokens = new List<Token>();
        var currentIndex = 0;
        var endOfLastToken = 0;

        do
        {
            var validParsers = parsers
                .Where(p => p.IsSymbolCanBeStartOfToken(text[currentIndex]) && !IsSymbolEscaped(text, currentIndex));

            if (!validParsers.Any())
            {
                currentIndex++;
                continue;
            }

            var parseResult = validParsers
                .Select(p => p.TryParseStringToToken(text[currentIndex..]))
                .Where(t => t.HasToken).FirstOrDefault();

            if (parseResult == null)
            {
                currentIndex++;
                continue;
            }

            var lastSymbol = currentIndex + parseResult.TokenLength - 1;
            if (IsSymbolEscaped(text, lastSymbol))
            {
                currentIndex++;
                continue;
            }

            if (text[endOfLastToken..currentIndex].Length != 0)
                tokens.Add(new SimpleToken(TokenType.SimpleText, text[endOfLastToken..currentIndex]));
            tokens.Add(parseResult.Token);
            currentIndex += parseResult.TokenLength;
            endOfLastToken = currentIndex;
        }
        while (currentIndex < text.Length);

        if (endOfLastToken != text.Length)
            tokens.Add(new SimpleToken(TokenType.SimpleText, text[endOfLastToken..]));

        return tokens;
    }

    private bool IsSymbolEscaped(string text, int indexOfSymbol)
    {
        if (indexOfSymbol == 0)
            return false;
        if (text[indexOfSymbol - 1] != '\\')
            return false;

        return !IsSymbolEscaped(text, indexOfSymbol - 1);
    }

    private int GetTokenLength(Token token)
    {
        if (token is not ComplexToken)
            return token.Content.Length;

        var result = 0;

        foreach (var child in token.Childs)
        {
            result += GetTokenLength(child);
        }

        return result;
    }
}
