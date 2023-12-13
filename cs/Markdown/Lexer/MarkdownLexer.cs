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

    //# __test__ _line_ asd d 
    public List<Token> Tokenize(string line)
    {
        if (line is null or "")
            throw new ArgumentException("Input parameter cannot be null or empty string.");
        
        var initialRegisteredTokens = PlaceRegisteredTokens(line);
        var validatedRegisteredTokens = validator.RemoveInvalidTokens(initialRegisteredTokens);
        var registeredTokensWithText = JoinTokensWithText(validatedRegisteredTokens, line);
        
        return registeredTokensWithText;
    }

    private List<Token> JoinTokensWithText(List<Token> validatedRegisteredTokensPos, string line)
    {
        throw new NotImplementedException();
    }

    private List<Token> PlaceRegisteredTokens(string line)
    {
        var registeredTokens = new List<Token>();
        var typeToPositions = new Dictionary<string, List<int>>();
        foreach (var tokenType in registeredTokenTypes)
            typeToPositions[tokenType.Key] = new List<int>();
        
        var currentIndex = -1;
        //text _em text_ text
        
        foreach (var tokenType in registeredTokenTypes)
        {
            while ((currentIndex = line.IndexOf(tokenType.Key, currentIndex + 1, StringComparison.Ordinal)) != -1)
            {
                typeToPositions[tokenType.Key].Add(currentIndex);
                
                registeredTokens.Add(new Token(
                    registeredTokenTypes[tokenType.Key],
                    typeToPositions[tokenType.Key].Count % 2 == 0,
                    currentIndex,
                    tokenType.Key.Length));
            }
        }

        return registeredTokens;
    }
}