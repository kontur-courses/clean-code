using System.Text;
using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown.Tokenizer;

public class Tokenizer : ITokenizer
{
    private readonly string text;
    private List<Token> Tokens { get; } = new();
    private StringBuilder LiteralBuilder { get; } = new();
    private StringBuilder PotentialToken { get; } = new();
    private Dictionary<string, Token> TokenDictionary { get; } = new();

    public Tokenizer(string str)
    {
        if (string.IsNullOrEmpty(str))
            throw new ArgumentException("Input string to tokenizer can not be null or empty");
        text = str;
    }

    public IEnumerable<Token> Tokenize()
    {
        for (var i = 0; i < text.Length; i++)
        {
            var symbol = text[i];

            if (symbol == '\n' || symbol == '\r')
            {
                CloseAllOpenedTokens(i-1);
                SaveLiteralToken(i);
                LiteralBuilder.Append(symbol);
                continue;
            }

            if (TokensGenerators.Generators.Keys.Any(key => key.StartsWith($"{PotentialToken}{symbol}")))
            {
                PotentialToken.Append(symbol);
                continue;
            }

            if (TokensGenerators.Generators.ContainsKey($"{PotentialToken}"))
            {
                CheckTokenAvailability(i);
                PotentialToken.Clear();
            }

            if (TokensGenerators.Generators.Keys.Any(key => key.StartsWith($"{symbol}")) && !CheckScreening(i))
            {
                
                PotentialToken.Append(symbol);
                continue;
            }

            LiteralBuilder.Append(symbol);
        }

        if (TokensGenerators.Generators.ContainsKey($"{PotentialToken}"))
        {
            CheckTokenAvailability(text.Length);
            PotentialToken.Clear();
        }

        SaveLiteralToken(text.Length);
        CloseAllOpenedTokens(text.Length - 1);

        return Tokens.Select(t =>
        {
            if (t.IsContented && !t.IsCorrect)
            {
                return t.ReplaceInvalidTokenToLiteral();
            }

            return new List<Token> { t };
        }).SelectMany(t => t);
    }

    private void CheckTokenAvailability(int index)
    {
        var separator = PotentialToken.ToString();
        var separatorStart = index - separator.Length;
        var separatorEnd = index - 1;
        
        if (TokenDictionary.ContainsKey(separator))
        {
            var token = TokenDictionary[separator];

            if (token.IsSingleSeparator)
            {
                if (Token.IsCorrectTokenOpenSeparator(separatorStart, separatorEnd, text) && token.CanCloseToken(separatorStart-1,text))
                {
                    token.CloseToken(separatorStart-1);
                    Tokens.Add(token);
                    TokenDictionary[separator] = TokensGenerators.Generators[separator].CreateToken(separatorStart);
                    return;
                }

                LiteralBuilder.Append(separator);
                return;
            }

            if (!Token.IsCorrectTokenCloseSeparator(separatorStart, separatorEnd,text))
            {
                LiteralBuilder.Append(separator);
                return;
            }

            SaveLiteralToken(separatorStart);
            
            token.CloseToken(separatorEnd);
            token.Validate(text, Tokens);

            CheckIntersectionAndSave(token);

            TokenDictionary.Remove(separator);

            return;
        }

        if (Token.IsCorrectTokenOpenSeparator(separatorStart,separatorEnd, text))
        {
            SaveLiteralToken(separatorStart);
            var token = TokensGenerators.Generators[separator].CreateToken(separatorStart);
            if (token.IsClosed)
            {
                Tokens.Add(token);
                return;
            }

            TokenDictionary[separator] = token;
            return;
        }

        LiteralBuilder.Append(separator);
    }

    private void CheckIntersectionAndSave(Token token)
    {
        if (token.IsCorrect || token.IsContented)
        {
            var intersectedToken = token.FindTokenIntersection(Tokens);
            if (intersectedToken!=null)
            {
                Tokens.Remove(intersectedToken);
                Tokens.AddRange(intersectedToken.ReplaceInvalidTokenToLiteral());
                Tokens.AddRange(token.ReplaceInvalidTokenToLiteral());
                return;
            }
            Tokens.Add(token);
            return;
        }

        Tokens.AddRange(token.ReplaceInvalidTokenToLiteral());
    }
    
    private void CloseAllOpenedTokens(int closeIndex)
    {
        foreach (var token in TokenDictionary.Values)
        {
            if (token.IsSingleSeparator)
            {
                token.CloseToken(closeIndex);
                token.Validate(text, Tokens);
                Tokens.Add(token);
                continue;
            }

            var literalToken = new LiteralToken(token.OpeningIndex, token.OpeningIndex + token.Separator.Length - 1,
                token.Separator);
            Tokens.Add(literalToken);
        }

        TokenDictionary.Clear();
    }

    private void SaveLiteralToken(int endIndex)
    {
        if (LiteralBuilder.Length != 0)
        {
            var literalToken =
                new LiteralToken(endIndex - LiteralBuilder.Length, endIndex - 1, LiteralBuilder.ToString());
            Tokens.Add(literalToken);
        }

        LiteralBuilder.Clear();
    }

    private bool CheckScreening(int index)
    {
        var screeningToken =
            Tokens.SingleOrDefault(token => token.ClosingIndex == index - 1 && token is ScreeningToken);

        if (screeningToken != null)
        {
            Tokens.Remove(screeningToken);
            return true;
        }

        return false;
    }
}