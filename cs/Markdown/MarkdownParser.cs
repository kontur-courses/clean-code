using System.Collections.Generic;
using System.Text;

namespace Markdown;

public class MarkdownParser(ILexer lexer) : IParserFrom
{
    Dictionary<TokenType, string> tokenSymbol = new()
    {
        [TokenType.BoldSymbol] = "__",
        [TokenType.ItalicSymbol] = "_",
        [TokenType.Header] = "#",
        [TokenType.Space] = " ",
    };

public List<MarkupToken> Parse(string markdownText)
    {
        var tokens = lexer.GetTokens(markdownText);
        var paragraph = new List<Token>();
        var result = new List<MarkupToken>();
        
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            paragraph.Add(token);
            if (paragraph.Count > 1 
                &&  paragraph[^2].Type == TokenType.NewLine
                &&  paragraph[^1].Type == TokenType.NewLine)
            {
                var handledParagraph = HandleParagraph(paragraph);
                result.AddRange(handledParagraph);
                paragraph.Clear();
            }
        }
        
        if (paragraph.Count > 0)
            result.AddRange(HandleParagraph(paragraph));

        return result;
    }

    private List<MarkupToken> HandleParagraph(List<Token> tokens)
    {
        RemoveEmptyUnderlines(tokens);
        var stack = new Stack<(TokenType Type, int index)>();
        for (var i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].Type == TokenType.Header && !IsValidHeader(tokens, i))
            {
                ToWordType(tokens, i);
            }
            
            else if (tokens[i].Type == TokenType.ItalicSymbol ||  tokens[i].Type == TokenType.BoldSymbol)
            {
                if (stack.Count > 0 && tokens[i].Type == stack.Peek().Type 
                                    && IsValidPair(tokens, stack.Peek().index, i))
                {
                    stack.Pop();
                    continue;
                }
                stack.Push((tokens[i].Type, i));
            }
        }

        while (stack.Count > 0)
        {
            var token = stack.Pop();
            ToWordType(tokens, token.index);
        }

        RemoveBoldIntoItalic(tokens);
        return ToMarkupTokens(tokens);
    }

    private bool IsValidHeader(List<Token> tokens, int i)
    {
        return ((i == 0 || tokens[i - 1].Type == TokenType.NewLine)  && tokens[i + 1].Type == TokenType.Space);
    }

    private List<MarkupToken> ToMarkupTokens(List<Token> tokens)
    {
        var result = new List<MarkupToken>();
        var buffer = new List<string>();
        var italicOpened = false;
        var boldOpened = false;
        var headerOpened = false;

        void SaveWord()
        {
            if (buffer.Count == 0)
                return;

            result.Add(new MarkupToken(
                MarkupTokenType.Word, string.Concat(buffer)));

            buffer.Clear();
        }

        foreach (var token in tokens)
        {
            if (token.Type == TokenType.NewLine && headerOpened)
            {
                result.Add(new MarkupToken(MarkupTokenType.CloseHeaderSymbol));
                headerOpened = false;
            }
            
            if (token.Type == TokenType.ItalicSymbol)
            {
                SaveWord();

                result.Add(new MarkupToken(italicOpened
                    ? MarkupTokenType.CloseItalicSymbol
                    : MarkupTokenType.OpenItalicSymbol));

                italicOpened = !italicOpened;
            }
            else if (token.Type == TokenType.BoldSymbol)
            {
                SaveWord();

                result.Add(new MarkupToken(boldOpened
                    ? MarkupTokenType.CloseBoldSymbol
                    : MarkupTokenType.OpenBoldSymbol));

                boldOpened = !boldOpened;
            }
            
            else if (token.Type == TokenType.Header)
            {
                result.Add(new MarkupToken(MarkupTokenType.OpenHeaderSymbol));
                headerOpened = true;
            }
            
            else
            {
                buffer.Add(token.Type switch
                {
                    TokenType.Space => " ",
                    _ => token.Value ?? ""
                });
            }
        }

        SaveWord();

        return result;
    }

    private void RemoveBoldIntoItalic(List<Token> tokens)
    {
        var insideItalic = false;

        for (var i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].Type == TokenType.ItalicSymbol)
            {
                insideItalic = !insideItalic;
            }
            else if (insideItalic && tokens[i].Type == TokenType.BoldSymbol)
            {
                ToWordType(tokens, i);
            }
        }
    }

    private bool IsValidPair(List<Token> tokens, int open, int close)
    {
        if (open < 0 || open == tokens.Count - 1)
            return false;

        if (tokens[open + 1].Type == TokenType.Space
            || tokens[open + 1].Type == TokenType.NewLine)
            return false;

        if (tokens[close - 1].Type == TokenType.Space
            || tokens[close - 1].Type == TokenType.NewLine)
            return false;

        if ((open > 0 && tokens[open - 1].Type == TokenType.Word)
            || (close + 1 < tokens.Count && tokens[close + 1].Type == TokenType.Word))
        {
            var subString = new StringBuilder();
            for (var i = open + 1; i < close; i++)
            {
                subString.Append(tokens[i].Value);
                if (tokens[i].Type == TokenType.Space
                    || tokens[i].Type == TokenType.NewLine)
                    return false;
            }
            if (int.TryParse(subString.ToString(), out int _)) return false;
        }

        return true;
    }

    private void RemoveEmptyUnderlines(List<Token> tokens)
    {
        var index = 0;

        while (index < tokens.Count)
        {
            if (tokens[index].Type != TokenType.BoldSymbol && tokens[index].Type != TokenType.ItalicSymbol)
            {
                index++;
                continue;
            }

            var start = index;
            while (index < tokens.Count
                   && tokens[index].Type is
                       (TokenType.ItalicSymbol or TokenType.BoldSymbol))
                index++;
            
            var leftBoundary = start == 0
                               || tokens[start - 1].Type ==
                                   TokenType.Space || tokens[start - 1].Type== TokenType.NewLine;

            var rightBoundary = index == tokens.Count
                                || tokens[index].Type == TokenType.Space 
                                || tokens[index].Type ==TokenType.NewLine;

            if (leftBoundary && rightBoundary)
            {
                ToWordType(tokens, start, index);
                index = start + 1;
            }
        }
    }

    private void ToWordType(List<Token> tokens, int start, int end)
    {
        for (var i = start; i < end; i++)
        {
            var token = tokens[i];

            if (tokenSymbol.TryGetValue(token.Type, out var value))
                tokens[i] = new Token(TokenType.Word, value ?? token.Value);

            tokens[i] = new Token(TokenType.Word, value);
        }
    }
    
    private void ToWordType(List<Token> tokens, int index)
    {
        var token = tokens[index];

        if (tokenSymbol.TryGetValue(token.Type, out var value))
            tokens[index] = new Token(TokenType.Word, value ?? token.Value);
    }
}
 
