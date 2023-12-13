using Markdown.Tokens;
using Markdown.Tokens.Types;
using Markdown.Validator;

namespace Markdown.Lexer;

public class MarkdownLexer : ILexer
{
    private readonly Dictionary<string, ITokenType> registeredTokenTypes = new();

    private readonly ITokenValidator validator;

    public MarkdownLexer(ITokenValidator validator)
    {
        this.validator = validator;
    }

    public IReadOnlyDictionary<string, ITokenType> RegisteredTokenTypes
        => registeredTokenTypes.AsReadOnly();

    public void RegisterTokenType(string typeSymbol, ITokenType type)
    {
        ValidateRegisterArguments(typeSymbol, type);

        if (registeredTokenTypes.ContainsKey(typeSymbol))
            throw new ArgumentException("The given type symbol has already been registered.");
        registeredTokenTypes.Add(typeSymbol, type);
    }

    // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
    private static void ValidateRegisterArguments(string typeSymbol, ITokenType type)
    {
        if (typeSymbol is null or "")
            throw new ArgumentException("Type symbol cannot be null or an empty string.");
        if (type is null)
            throw new ArgumentException("Token type cannot be null");
        if (type.Representation(true) is null || type.Representation(false) is null)
            throw new ArgumentException("Token type representation cannot be null.");
    }


    public List<Token> Tokenize(string line)
    {
        if (line is null or "")
            throw new ArgumentException("Input parameter cannot be null or empty string.");

        var initialRegisteredTokens = PlaceRegisteredTokens(line);
        var validatedRegisteredTokens = validator.RemoveInvalidTokens(initialRegisteredTokens);
        var registeredTokensWithText = JoinTokensWithText(validatedRegisteredTokens, line);

        return registeredTokensWithText;
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
        if (lastLineLength - 1 > 0)
            joinedWithText.Add(new Token(new TextToken(line.Substring(lastIndex, lastLineLength)), false, lastIndex,
                lastLineLength));

        return joinedWithText;
    }

    private List<Token> PlaceRegisteredTokens(string line)
    {
        var registeredTokens = new List<Token>();
        var placedTokensNumber = registeredTokenTypes
            .ToDictionary(type => type.Key, _ => 0);

        var occupiedPositions = new HashSet<int>();
        foreach (var tokenType in registeredTokenTypes.OrderByDescending(t => t.Key.Length))
        {
            var currentIndex = -1;
            while ((currentIndex = GetNextIndexOf(line, tokenType.Key, currentIndex)) != -1)
            {
                if (occupiedPositions.Contains(currentIndex) ||
                    BeginningSemanticsNotFulfilled(tokenType.Value.HasLineBeginningSemantics, currentIndex))
                    continue;

                registeredTokens.Add(new Token(
                    registeredTokenTypes[tokenType.Key],
                    tokenType.Value.ValueSupportsClosingTag && ++placedTokensNumber[tokenType.Key] % 2 == 0,
                    currentIndex,
                    tokenType.Key.Length));

                for (var toOccupy = 0; toOccupy < tokenType.Key.Length; toOccupy++)
                    occupiedPositions.Add(currentIndex + toOccupy);
            }
        }

        return registeredTokens
            .OrderBy(t => t.StartingIndex)
            .ToList();
    }

    private static bool BeginningSemanticsNotFulfilled(bool hasBeginningSemantics, int currentIndex)
        => hasBeginningSemantics && currentIndex != 0;

    private static int GetNextIndexOf(string line, string substr, int currentIndex)
        => line.IndexOf(substr, currentIndex + 1, StringComparison.Ordinal);
}