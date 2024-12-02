using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Markdown.Tokenizer.Tokens;
using Markdown.Tokenizer.TokenScanners;

namespace Markdown.Tokenizer;
public class MdTokenizer : IMdTokenizer
{
    private static readonly List<ITokenScanner> TokenScanners;
    
    static MdTokenizer()
    {
        TokenScanners = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(ITokenScanner).IsAssignableFrom(t) && t.IsClass)
            .Select(Activator.CreateInstance)
            .Cast<ITokenScanner>()
            .ToList();
    }
    
    public TokenList Tokenize(string text)
    {
        var tokensAsLinkedList = TokensAsLinkedList(text);
        
        tokensAsLinkedList.AddFirst(new Token(TypeOfToken.StartOfParagraph));
        tokensAsLinkedList.AddLast(new Token(TypeOfToken.EndOfParagraph));
        
        return new TokenList(tokensAsLinkedList);
    }

    private static LinkedList<Token> TokensAsLinkedList(string text)
    {
        var remainingTokens = new LinkedList<Token>();
        
        if (string.IsNullOrEmpty(text))
        {
            return remainingTokens;
        }

        for (var i = 0; i < text.Length;)
        {
            var token = ScanOneToken(text, i);
            
            if (token.Type == TypeOfToken.Newline)
            {
                remainingTokens.AddLast(new Token(TypeOfToken.EndOfParagraph));
                remainingTokens.AddLast(token);
                remainingTokens.AddLast(new Token(TypeOfToken.StartOfParagraph));
            }
            else
            {
                remainingTokens.AddLast(token);
            }

            i += token.Length;
        }
        
        return remainingTokens;
    }
    
    private static Token ScanOneToken(string markdown, int index)
    {
        foreach (var scanner in TokenScanners)
        {
            var isToken = scanner.TryGetToken(markdown, index, out var token);
            
            if (isToken && token != null)
            {
                return token;
            }
        }
        
        throw new Exception($"The scanners could not match the given input: {markdown}");
    }
}