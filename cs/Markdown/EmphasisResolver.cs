using Markdown.Tokens;

namespace Markdown;
 
/// <summary>
/// Проставляет Token.Role разделителям за один линейный проход
/// читает и меняет Token, для LinkToken рекурсивно разбирает Content
/// </summary>

internal sealed class EmphasisResolver
{
    private readonly string text;
    private readonly List<Token> tokens;
    private readonly int[] whitespacesBefore;
    
    private readonly Stack<int> openedEm = new();
    private readonly Stack<int> openedStrong = new();
    private readonly List<(int Open, int Close)> strongPairs = new();

    private EmphasisResolver(string text, List<Token> tokens)
    {
        this.text = text;
        this.tokens = tokens;
        whitespacesBefore = CountWhitespaces(text);
    }

    public static void Resolve(string text, List<Token> tokens)
        => new EmphasisResolver(text, tokens).Run();

    private void Run()
    {
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (token.IsDelimiter)
                Process(i);
            else if (token is LinkToken link)
                Resolve(link.Text, link.Content); 
        }
    }

    private void Process(int index)
    {
        var token = tokens[index];
        if (IsInsideTextWithDigits(token))
            return;

        var opened = OpenedOf(token);
        if (CanClose(token) && opened.Count > 0 && TryClose(index, opened))
            return;

        if (CanOpen(token))
            opened.Push(index);
    }
    private bool IsInsideTextWithDigits(Token token)
    {
        throw new NotImplementedException();
    }

    private bool CanOpen(Token token)
    {
        throw new NotImplementedException();
    }
        
    
    private bool CanClose(Token token)
    {
        throw new NotImplementedException();
    }
    
    private bool TryClose(int closeIndex, Stack<int> opened)
    {
        var openIndex = opened.Peek();
        if (!IsValidPair(tokens[openIndex], tokens[closeIndex]))
            return false;

        opened.Pop();

        var others = OtherOf(tokens[closeIndex]);
        if (others.Count > 0 && others.Peek() > openIndex)
        {
            // ни один из пересекающихся разделителей не становится выделением
            while (others.Count > 0 && others.Peek() > openIndex)
                others.Pop();
            return true;
        }

        MarkPair(openIndex, closeIndex);
        return true;
    }
    
    private void MarkPair(int openIndex, int closeIndex)
    {
        throw new NotImplementedException();
    }
    
    private bool IsValidPair(Token open, Token close)
    {
        throw new NotImplementedException();
    }
    
    private static int[] CountWhitespaces(string text)
    {
        throw new NotImplementedException();
    }
    
    private Stack<int> OpenedOf(Token token)
        => token.Kind == TokenKind.Underscore ? openedEm : openedStrong;

    private Stack<int> OtherOf(Token token)
        => token.Kind == TokenKind.Underscore ? openedStrong : openedEm;
}