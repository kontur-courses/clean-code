using System.Text;
using Markdown.Tokens;

namespace Markdown.Tokenizer;

public class Tokenizer : ITokenizer
{
    private readonly Dictionary<string, Token> tokensDictionary = new();
    private readonly List<Token> tokenList = new();
    private readonly StringBuilder literalTokenBuilder = new();
    private readonly List<int> screeningIndex = new();
    private int literalIndex;

    public IEnumerable<Token> Tokenize(string str)
    {
        if (string.IsNullOrEmpty(str))
            throw new ArgumentException();
        var potentialToken = new StringBuilder();
        for (var i = 0; i < str.Length; i++)
        {
            var symbol = str[i];

            if (symbol == '\n')
                CloseAllOpenedTokensInTheEnd(i);

            if (symbol == '\\')
            {
                if (screeningIndex.Contains(i - 1))
                {
                    screeningIndex.Remove(i - 1);
                    literalTokenBuilder.Append(symbol);
                    continue;
                }
                SaveLiteralToken(i-1,i+1);
                screeningIndex.Add(i);
                continue;
            }

            if (TokensGenerators.TokenGenerators.Any(token => token.Key.StartsWith($"{potentialToken}{symbol}")))
            {
                potentialToken.Append(symbol);
                continue;
            }

            if (TokensGenerators.TokenGenerators.ContainsKey($"{potentialToken}"))
            {
                CheckTokenAvailability($"{potentialToken}", i);
                potentialToken.Clear();
            }

            literalTokenBuilder.Append(potentialToken);
            potentialToken.Clear();
            literalTokenBuilder.Append(symbol);
        }

        if (potentialToken.Length > 0)
            CheckTokenAvailability(potentialToken.ToString(), str.Length - 1);
        CloseAllOpenedTokensInTheEnd(str.Length - 1);
        AddNotImplementedScreening();
        SaveLiteralToken(str.Length - 1, str.Length);
        return tokenList;
    }


    private void CheckTokenAvailability(string separator, int index)
    {
        var tokenGenerator = TokensGenerators.TokenGenerators[separator];
        var previousIndex = tokenGenerator.GetPreviousIndex(index);
        var startTokenIndex = tokenGenerator.GetTokenStartIndex(index);
        
        if (CheckIsTokenScreening(previousIndex))
        {
            literalTokenBuilder.Append(separator);
            
            return;
        }

        if (tokensDictionary.ContainsKey(separator))
        {
            SaveLiteralToken(previousIndex, index);
            var token = tokensDictionary[separator];
            token.CloseToken(index-1);
            tokenList.Add(token);
            if (tokenGenerator.IsSingleSeparator)
            {
                tokensDictionary[separator] = tokenGenerator.CreateToken(startTokenIndex);
                return;
            }

            tokensDictionary.Remove(separator);
            return;
        }

        tokensDictionary[separator] = tokenGenerator.CreateToken(startTokenIndex);
        SaveLiteralToken(previousIndex, index);
    }

    private bool CheckIsTokenScreening(int index)
    {
        if (screeningIndex.Contains(index))
        {
            screeningIndex.Remove(index);
            return true;
        }

        return false;
    }

    private void AddNotImplementedScreening()
    {
        foreach (var index in screeningIndex)
        {
            tokenList.Add(new LiteralToken(index, index, '\\'.ToString()));
        }   
    }

    private void CloseAllOpenedTokensInTheEnd(int endIndex)
    {
        foreach (var token in tokensDictionary.Values)
        {
            if (token.IsTokenSingleSeparator())
            {
                token.CloseToken(endIndex);
                tokenList.Add(token);
                continue;
            }

            var literalToken = new LiteralToken(token.OpeningIndex,
                token.OpeningIndex + token.GetSeparator().Length - 1,
                token.GetSeparator());
            tokenList.Add(literalToken);
        }
    }

    private void SaveLiteralToken(int endIndex, int newIndex)
    {
        if (literalTokenBuilder.Length != 0)
        {
            var literalToken = new LiteralToken(literalIndex, endIndex, literalTokenBuilder.ToString());
            tokenList.Add(literalToken);
        }

        literalTokenBuilder.Clear();
        literalIndex = newIndex;
    }
}