﻿using System.Text;
using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown.Tokenizer;

public class Tokenizer : ITokenizer
{
    private readonly string text;
    private List<Token> Tokens { get; } = new();
    private StringBuilder LiteralBuilder { get; } = new();
    private Dictionary<string, Token> OpenedTokens { get; } = new();

    public Tokenizer(string str)
    {
        if (string.IsNullOrEmpty(str))
            throw new ArgumentException("Input string to tokenizer can not be null or empty");
        text = str;
    }

    public IEnumerable<Token> Tokenize()
    {
        var potentialToken = new StringBuilder();
        for (var i = 0; i < text.Length; i++)
        {
            var symbol = text[i];

            if (symbol == '\n' || symbol == '\r')
            {
                CloseAllOpenedTokens(i - 1);
                SaveLiteralToken(i);
                LiteralBuilder.Append(symbol);
                continue;
            }

            if (TokensGenerators.Generators.Keys.Any(key => key.StartsWith($"{potentialToken}{symbol}")))
            {
                potentialToken.Append(symbol);
                continue;
            }

            if (TokensGenerators.Generators.ContainsKey($"{potentialToken}"))
            {
                CheckTokenAvailability(i, potentialToken.ToString());
                potentialToken.Clear();
            }

            if (TokensGenerators.Generators.Keys.Any(key => key.StartsWith($"{symbol}")) && !CheckScreening(i))
            {
                potentialToken.Append(symbol);
                continue;
            }

            LiteralBuilder.Append(symbol);
        }

        if (TokensGenerators.Generators.ContainsKey($"{potentialToken}"))
        {
            CheckTokenAvailability(text.Length, potentialToken.ToString());
            potentialToken.Clear();
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

    private void CheckTokenAvailability(int index, string separator)
    {
        var separatorStart = index - separator.Length;
        var separatorEnd = index - 1;

        if (OpenedTokens.ContainsKey(separator))
        {
            var token = OpenedTokens[separator];

            if (token.IsSingleSeparator)
            {
                if (IsCorrectTokenOpenSeparator(separatorStart, separatorEnd, text) &&
                    token.CanCloseToken(separatorStart - 1, text))
                {
                    token.CloseToken(separatorStart - 1);
                    Tokens.Add(token);
                    OpenedTokens[separator] = TokensGenerators.Generators[separator](separatorStart);
                    return;
                }

                LiteralBuilder.Append(separator);
                return;
            }

            if (!IsCorrectTokenCloseSeparator(separatorStart, separatorEnd, text))
            {
                LiteralBuilder.Append(separator);
                return;
            }

            SaveLiteralToken(separatorStart);

            token.CloseToken(separatorEnd);
            token.Validate(text, Tokens);

            CheckIntersectionAndSave(token);

            OpenedTokens.Remove(separator);

            return;
        }

        if (IsCorrectTokenOpenSeparator(separatorStart, separatorEnd, text))
        {
            SaveLiteralToken(separatorStart);
            var token = TokensGenerators.Generators[separator](separatorStart);
            if (token.IsClosed)
            {
                Tokens.Add(token);
                return;
            }

            OpenedTokens[separator] = token;
            return;
        }

        LiteralBuilder.Append(separator);
    }

    private void CheckIntersectionAndSave(Token token)
    {
        if (token.IsCorrect || token.IsContented)
        {
            var intersectedToken = token.FindTokenIntersection(Tokens);
            if (intersectedToken != null)
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
        foreach (var token in OpenedTokens.Values)
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

        OpenedTokens.Clear();
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

    private static bool IsCorrectTokenOpenSeparator(int separatorStart, int separatorEnd, string str)
    {
        return separatorEnd < str.Length - 1 && str[separatorEnd + 1] != ' ' &&
               !IsSeparatorInsideDigit(separatorStart, separatorEnd, str);
    }

    private static bool IsCorrectTokenCloseSeparator(int separatorStart, int separatorEnd, string str)
    {
        return separatorEnd != 0 && str[separatorStart - 1] != ' ' &&
               !IsSeparatorInsideDigit(separatorStart, separatorEnd, str);
    }

    private static bool IsSeparatorInsideDigit(int separatorStart, int separatorEnd, string str)
    {
        var isLeftDigit = (separatorStart > 0 && char.IsDigit(str[separatorStart - 1]));
        var isRightDigit = (separatorEnd < str.Length - 1 && char.IsDigit(str[separatorEnd + 1]));
        return isLeftDigit || isRightDigit;
    }
}