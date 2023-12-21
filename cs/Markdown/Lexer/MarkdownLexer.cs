using Markdown.Filter;
using Markdown.Tokens;
using Markdown.Tokens.Types;

namespace Markdown.Lexer;

public class MarkdownLexer : ILexer
{
    private readonly Dictionary<string, ITokenType> registeredTokenTypes = new();

    private readonly ITokenFilter filter;

    private readonly char escapeSymbol;

    public MarkdownLexer(ITokenFilter filter, IReadOnlySet<ITokenType> tokenTypes, char escapeSymbol = '\\')
    {
        this.escapeSymbol = escapeSymbol;
        this.filter = filter ?? throw new ArgumentException("Filter cannot be null.");
        RegisterTokenTypes(tokenTypes);
    }

    public IReadOnlyDictionary<string, ITokenType> RegisteredTokenTypes
        => registeredTokenTypes.AsReadOnly();

    private void RegisterTokenTypes(IReadOnlySet<ITokenType> tokenTypes)
    {
        foreach (var tokenType in tokenTypes)
            registeredTokenTypes.Add(tokenType.Value, tokenType);
    }

    public TokenizeResult Tokenize(string line)
    {
        if (line is null)
            throw new ArgumentException("Input parameter cannot be null.");

        var escapedEscapeSymbolsPos = EscapeSymbolProcessor.GetEscapedEscapeSymbolsPositions(line, escapeSymbol);
        var initialRegisteredTokens = PlaceRegisteredTokens(line, escapedEscapeSymbolsPos);
        
        var filteredRegisteredTokens = filter.FilterTokens(initialRegisteredTokens.Tokens, line);
        
        var registeredTokensWithText = JoinTokensWithText(filteredRegisteredTokens, line);

        return new TokenizeResult(registeredTokensWithText, initialRegisteredTokens.EscapeSymbolsPos);
    }

    private static List<Token> JoinTokensWithText(List<Token> validatedRegisteredTokens, string line)
    {
        var joinedWithText = new List<Token>();
        var lastIndex = 0;

        foreach (var registeredToken in validatedRegisteredTokens)
        {
            var currentSubstrLength = registeredToken.StartingIndex - lastIndex;
            if (currentSubstrLength > 0)
                joinedWithText.Add(new Token(new TextToken(line.Substring(lastIndex, currentSubstrLength)), false,
                    lastIndex, currentSubstrLength));

            joinedWithText.Add(registeredToken);
            lastIndex = registeredToken.StartingIndex + registeredToken.Length;
        }

        var lastLineLength = line.Length - lastIndex;
        if (lastLineLength > 0)
            joinedWithText.Add(new Token(new TextToken(line.Substring(lastIndex, lastLineLength)), false, lastIndex,
                lastLineLength));

        return joinedWithText;
    }

    private bool IsTokenEscaped(string line, int currentIndex, IReadOnlyDictionary<int, bool> escapedEscapeSymbolsPos)
        => currentIndex != 0
           && line[currentIndex - 1] == escapeSymbol
           && !escapedEscapeSymbolsPos.ContainsKey(currentIndex - 1);

    private TokenizeResult PlaceRegisteredTokens(string line, IReadOnlyDictionary<int, bool> escapedEscapeSymbolsPos)
    {
        var registeredTokens = new List<Token>();
        var placedTokensNumber = registeredTokenTypes
            .ToDictionary(type => type.Key, _ => 0);

        var escapeSymbolsPos = escapedEscapeSymbolsPos.ToDictionary(e => e.Key, e => e.Value);
        var occupiedPositions = new HashSet<int>();
        foreach (var (tokenValue, tokenType) in registeredTokenTypes.OrderByDescending(t => t.Key.Length))
        {
            var currentIndex = -1;
            while ((currentIndex = GetNextIndexOf(line, tokenValue, currentIndex)) != -1)
            {
                if (occupiedPositions.Contains(currentIndex) ||
                    BeginningSemanticsNotFulfilled(tokenType.HasLineBeginningSemantics, currentIndex))
                    continue;

                if (IsTokenEscaped(line, currentIndex, escapeSymbolsPos))
                {
                    occupiedPositions.Add(currentIndex);
                    escapeSymbolsPos.Add(currentIndex - 1, true);
                    continue;
                }

                registeredTokens.Add(new Token(
                    registeredTokenTypes[tokenValue],
                    tokenType.SupportsClosingTag && ++placedTokensNumber[tokenValue] % 2 == 0,
                    currentIndex,
                    tokenValue.Length));

                for (var toOccupy = 0; toOccupy < tokenValue.Length; toOccupy++)
                    occupiedPositions.Add(currentIndex + toOccupy);
            }
        }

        return new TokenizeResult(registeredTokens
            .OrderBy(t => t.StartingIndex)
            .ToList(), escapeSymbolsPos);
    }

    private static bool BeginningSemanticsNotFulfilled(bool hasBeginningSemantics, int currentIndex)
        => hasBeginningSemantics && currentIndex != 0;

    private static int GetNextIndexOf(string line, string substr, int currentIndex)
        => line.IndexOf(substr, currentIndex + 1, StringComparison.Ordinal);
}