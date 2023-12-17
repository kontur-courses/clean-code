using System.Text;
using Markdown.Tokens;

namespace Markdown.Tokenizer;

public class Tokenizer : ITokenizer
{
    private readonly Dictionary<string, int> tokensDictionary;
    private readonly List<Token> tokenList;
    private readonly StringBuilder literalTokenBuilder;
    private int literalIndex;

    public Tokenizer()
    {
        tokensDictionary = new Dictionary<string, int>();
        tokenList = new List<Token>();
        literalTokenBuilder = new StringBuilder();
    }

    public IEnumerable<Token> Tokenize(string str)
    {
        var isScreening = false;
        var screeningTxt = new StringBuilder();
        var potentialToken = new StringBuilder();
        for (var i = 0; i < str.Length; i++)
        {
            var symbol = str[i];

            if (symbol == '\\')
            {
                isScreening = true;
                continue;
            }

            if (isScreening)
            {
                if (TokensGenerators.TokenGenerators.Any(token => token.Key.StartsWith($"{screeningTxt}{symbol}")))
                {
                    screeningTxt.Append(symbol);
                    continue;
                }

                if (TokensGenerators.TokenGenerators.ContainsKey($"{screeningTxt}"))
                {
                    literalTokenBuilder.Append(screeningTxt);
                    screeningTxt.Clear();
                }

                isScreening = false;
                literalTokenBuilder.Append(screeningTxt);
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
        SaveLiteralToken(str.Length-1, str.Length);
        return tokenList;
    }
    
    private void CheckTokenAvailability(string separator, int index)
    {
        var tokenGenerator = TokensGenerators.TokenGenerators[separator];
        if (tokensDictionary.ContainsKey(separator))
        {
            SaveLiteralToken(tokenGenerator.GetPreviosIndex(index), index);
            tokenList.Add(tokenGenerator.CreateToken(tokensDictionary[separator], index-1));
            if (tokenGenerator.IsSingleSeparator)
            {
                tokensDictionary[separator] = index;
                return;
            }
            tokensDictionary.Remove(separator);
        }
        else
        {
            tokensDictionary[separator] = tokenGenerator.GetTokenStartIndex(index);
            SaveLiteralToken( tokenGenerator.GetPreviosIndex(index), index);
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
    
    public IEnumerable<Token> CreateToken(IEnumerable<Token> tokens)
    {
        throw new NotImplementedException();
    }
}