using Markdown.Tokens;

namespace Markdown.EmphasisResolving;

public sealed class EmphasisResolver
{
    private int[] whitespacesBefore;
    
    public void Resolve(List<Token> tokens, string text)
    {
        if (whitespacesBefore == null)
            whitespacesBefore = CountWhitespaces(text);
        
        var parents = new Stack<Scope>();
        var scope = new Scope();

        for (var position = 0; position < tokens.Count; position++)
        {
            var token = tokens[position];

            switch (token)
            {
                case UnderscoreToken or DoubleUnderscoreToken:
                    Process(position, tokens, text, scope);
                    break;

                case LineFeedToken:
                    scope = new Scope();
                    break;

                case LinkToken or HeaderToken:
                    if (token.Role == TokenRole.Opening)
                    {
                        parents.Push(scope);
                        scope = new Scope();
                    }
                    else if (token.Role == TokenRole.Closing)
                    {
                        if (parents.Count == 0)
                            return;
                        scope = parents.Pop();
                    }
                    break;
            }
        }
    }

    private void Process(int index, List<Token> tokens, string text, Scope scope)
    {
        var token = tokens[index];
        if (IsInsideTextWithDigits(token, text))
            return;

        var opened = OpenedOf(token, scope);
        if (CanClose(token, text) 
            && TryGetPair(tokens, index, opened, text, out var openIndex)
            && TryClose(tokens, index, openIndex, scope))
        {
            tokens[openIndex].Role = TokenRole.Opening;
            tokens[index].Role = TokenRole.Closing;
            
            if (tokens[openIndex] is DoubleUnderscoreToken)
                scope.StrongPairs.Add((openIndex, index));
            else
                DemoteStrongInside(tokens, openIndex, scope);
        }
        
        else if (CanOpen(token, text))
            opened.Push(index);
    }

    private bool TryGetPair(List<Token> tokens, int closeIndex, Stack<int> opened, string text, out int openIndex)
    {
        if (!opened.TryPeek(out openIndex))
            return false;
        
        var open = tokens[openIndex];
        var close = tokens[closeIndex];
        
        var touchesWord = IsLetterOrDigitAt(open.Start - 1, text) || IsLetterOrDigitAt(close.End, text);
        if (!touchesWord || whitespacesBefore[close.Start] - whitespacesBefore[open.End] == 0)
        {
            opened.Pop();
            return true;
        }

        return false;
    }

    private bool TryClose(List<Token> tokens, int closeIndex, int openIndex, Scope scope)
    {
        var others = OtherOf(tokens[closeIndex], scope);
        if (others.Count > 0 && others.Peek() > openIndex)
        {
            while (others.Count > 0 && others.Peek() > openIndex)
                others.Pop();
            return false;
        }
        
        return true;
    }
    

    private void DemoteStrongInside(List<Token> tokens, int emOpenIndex, Scope scope)
    {
        var pairs = scope.StrongPairs;
        while (pairs.Count > 0 && pairs[^1].Open > emOpenIndex)
        {
            var (open, close) = pairs[^1];
            tokens[open].Role = TokenRole.Literal;
            tokens[close].Role = TokenRole.Literal;
            pairs.RemoveAt(pairs.Count - 1);
        }
    }

    private static Stack<int> OpenedOf(Token token, Scope scope)
        => token is UnderscoreToken ? scope.OpenedEm : scope.OpenedStrong;

    private static Stack<int> OtherOf(Token token, Scope scope)
        => token is UnderscoreToken ? scope.OpenedStrong : scope.OpenedEm;

    private bool CanOpen(Token token, string text)
        => token.End < text.Length && !char.IsWhiteSpace(text[token.End]);

    private bool CanClose(Token token, string text)
        => token.Start > 0 && !char.IsWhiteSpace(text[token.Start - 1]);

    private bool IsInsideTextWithDigits(Token token, string text)
    {
        var before = token.Start > 0 ? text[token.Start - 1] : '\0';
        var after = token.End < text.Length ? text[token.End] : '\0';
        return (char.IsDigit(before) && char.IsLetterOrDigit(after))
               || (char.IsDigit(after) && char.IsLetterOrDigit(before));
    }

    private bool IsLetterOrDigitAt(int index, string text)
        => index >= 0 && index < text.Length && char.IsLetterOrDigit(text[index]);

    private static int[] CountWhitespaces(string text)
    {
        var counts = new int[text.Length + 1];
        for (var i = 0; i < text.Length; i++)
            counts[i + 1] = counts[i] + (char.IsWhiteSpace(text[i]) ? 1 : 0);
        return counts;
    }
}