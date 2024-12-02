using Markdown.Enums;
using Markdown.Tokens;

namespace Markdown.Tokenizers;

public class Tokenizer : ITokenizer
{
    private List<Token> tokens = [];
    public List<Token> SplitToTokens(string text)
    {
        var result = new List<Token>();
        var paragraphs = text.Split('\n');
        for (var i = 0; i < paragraphs.Length; i++)
        {
            tokens = GetTokens(paragraphs[i]);
            tokens = ProcessScreeningTokens(tokens);
            ProcessInvalidTokens();
            ProcessNonPairTokens();
            ProcessWrongOrder();
            result.AddRange(tokens);
            if (paragraphs.Length > 1 && i < paragraphs.Length - 1)
                result.Add(new LiteralToken("\n", LiteralType.Text));
            tokens.Clear();
        }
        return result;
    }

    private static List<Token> GetTokens(string text)
    {
        var tokens = new List<Token>();
        var index = 0;
        while (index < text.Length)
        {
            tokens.Add(TokenFactory.GenerateToken(text, index));
            index += tokens.Last().Content.Length;
        }
        if (tokens.FirstOrDefault() is ParagraphToken)
            tokens.Add(new ParagraphToken(tokens.First().Content));
        return tokens;
    }
    
    private static List<Token> ProcessScreeningTokens(List<Token> tokens)
    {
        Token? previousToken = null;
        var result = new List<Token>();
        foreach (var token in tokens)
        {
            if (previousToken is ScreeningToken)
            {
                if (!token.IsTag && token is not ScreeningToken)
                {
                    var newToken = new LiteralToken(previousToken);
                    result.Add(newToken);
                    result.Add(token);
                    previousToken = token;
                    
                }
                else
                {
                    var newToken = new LiteralToken(token);
                    previousToken = newToken;
                    result.Add(newToken);
                }
            }
            else
            {
                if (token is not ScreeningToken)
                    result.Add(token);
                previousToken = token;
            }
        }

        if (previousToken is not ScreeningToken) 
            return result;
        result.Add(new LiteralToken(previousToken));
        return result;
    }

    private void ProcessInvalidTokens()
    {
        for (var i = 0; i < tokens.Count; i++)
            tokens[i] = tokens[i].Validate(tokens, i) ? tokens[i] : new LiteralToken(tokens[i]);
    }
    
    private void ProcessNonPairTokens()
    {
        var openTokensIndexes = new Stack<int>();
        var incorrectTokensIndexes = new List<int>();
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (!token.IsTag) 
                continue;
            if (token.IsOpen(tokens, i))
                openTokensIndexes.Push(i);
            else
            {
                if (openTokensIndexes.Count == 0)
                    incorrectTokensIndexes.Add(i);
                else
                    CheckOpenAndCloseTokens(openTokensIndexes, openTokensIndexes.Pop(), i, incorrectTokensIndexes);
            }
        }
        incorrectTokensIndexes.AddRange(openTokensIndexes);
        
        foreach (var index in incorrectTokensIndexes)
            tokens[index] = new LiteralToken(tokens[index]);
    }
    
    private void ProcessWrongOrder()
    {
        var openedTokens = new Stack<Token>();
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (!token.IsTag) continue;
            if (token.IsClosing)
                openedTokens.Pop();
            else
                openedTokens.Push(token);
            if (IsCorrectOrder(openedTokens, token)) 
                continue;
            tokens[i] = new LiteralToken(token);
            tokens[tokens[i].PairedTokenIndex] = new LiteralToken(tokens[tokens[i].PairedTokenIndex]);
        }
    }
    
    private static bool IsCorrectOrder(Stack<Token> openedTokens, Token token)
        => token is not BoldToken || openedTokens.All(x => x is not ItalicsToken);
    
    private void CheckOpenAndCloseTokens(Stack<int> openTokens, int openIndex, int closeIndex, List<int> incorrectTokens)
    {
        var openToken = tokens[openIndex];
        var closeToken = tokens[closeIndex];
        closeToken.IsClosing = true; 
        if (openToken.GetType() == closeToken.GetType()) 
        { 
            SetPairedTokens(openIndex, closeIndex);
            return;
        }
        var nextIndex = GetNextTokenIndex(closeIndex);
        if (openTokens.Count == 0)
        {
            if (nextIndex > -1)
            {
                if (tokens[nextIndex].IsOpen(tokens, nextIndex) || 
                    tokens[nextIndex].GetType() == tokens[openIndex].GetType())
                {
                    openTokens.Push(openIndex);
                    incorrectTokens.Add(closeIndex);
                }
                if (tokens[nextIndex].GetType() == tokens[openIndex].GetType())
                    return;
            }
            incorrectTokens.Add(openIndex);
            incorrectTokens.Add(closeIndex);
            return;
        }
        var preOpenIndex = openTokens.Peek();
        if (tokens[preOpenIndex].GetType() != closeToken.GetType()) 
            return;
        if (nextIndex > -1 && !tokens[nextIndex].IsOpen(tokens, nextIndex)
                       && tokens[nextIndex].GetType() == openToken.GetType())
        { 
            openTokens.Pop();
            incorrectTokens.Add(preOpenIndex);
            incorrectTokens.Add(openIndex);
            incorrectTokens.Add(closeIndex);
            return;
        }

        openTokens.Pop();
        SetPairedTokens(preOpenIndex, closeIndex);
        incorrectTokens.Add(openIndex);
    }

    private void SetPairedTokens(int openIndex, int closeIndex)
    {
        tokens[openIndex].PairedTokenIndex = closeIndex; 
        tokens[closeIndex].PairedTokenIndex = openIndex; 
    }
     
    private int GetNextTokenIndex(int index)
    {
        for (var i = index + 1; i < tokens.Count; i++)
            if (tokens[i].IsTag) return i;
        return -1;
    }
}