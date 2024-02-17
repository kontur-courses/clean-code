using System.Runtime.InteropServices;
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

            if (symbol == '\n')
                CloseAllOpenedTokens(i);

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

            if (TokensGenerators.Generators.Keys.Any(key => key.StartsWith($"{symbol}")))
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
        return Tokens;
    }

    private void CheckTokenAvailability(int index)
    {
        var separator = PotentialToken.ToString();
        var separatorStart = index - separator.Length;
        var separatorEnd = index - 1;

        if (CheckScreening(separatorStart) || Token.IsSeparatorInsideDigit(separatorStart, separatorEnd, text))
        {
            LiteralBuilder.Append(separator);
            return;
        }

        if (TokenDictionary.ContainsKey(separator) && Token.IsCorrectTokenCloseIndex(separatorStart, text))
        {
            SaveLiteralToken(separatorStart);
            
            var token = TokenDictionary[separator];
            token.CloseToken(separatorEnd);
            token.Validate(text);
            
           /* if (token.IsCorrect)
            {
                Tokens.Add(token);
            }
            else Tokens.AddRange(token.ReplaceInvalidTokenToLiteral());
            */
            CheckIntersectionAndSave(token);

            TokenDictionary.Remove(separator);
            return;
        }

        if (Token.IsCorrectTokenOpenIndex(separatorEnd, text))
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
        if (token.IsCorrect)
        {
            var intersections = Tokens.Where(t => t.OpeningIndex < token.OpeningIndex
                                                  && t.ClosingIndex > token.OpeningIndex
                                                  && t.ClosingIndex < token.ClosingIndex && !(t is LiteralToken)).ToArray();
            if (intersections.Any())
            {
                foreach (var intersect in intersections)
                {
                    Tokens.Remove(intersect);
                    Tokens.AddRange(intersect.ReplaceInvalidTokenToLiteral());
                }
                Tokens.AddRange(token.ReplaceInvalidTokenToLiteral());
                return;
            }
            Tokens.Add(token);
            return;
        }
        Tokens.AddRange(token.ReplaceInvalidTokenToLiteral());
    }
    
    private List<Token> FindIntersections(Token token)
    {
        return Tokens.Where(t =>
            t.OpeningIndex < token.OpeningIndex && t.ClosingIndex > token.OpeningIndex &&
            t.ClosingIndex < token.ClosingIndex).ToList();
    }

    private void CloseAllOpenedTokens(int closeIndex)
    {
        foreach (var token in TokenDictionary.Values)
        {
            if (token.IsSingleSeparator)
            {
                token.CloseToken(closeIndex);
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