using Markdown.Tokens;

namespace Markdown.EmphasisResolving;

internal sealed class EmphasisResolver
{
    private readonly string text;
    private readonly List<Token> tokens;
    private readonly int[] whitespacesBefore;

    private EmphasisResolver(string text, List<Token> tokens, int[] whitespacesBefore)
    {
        this.text = text;
        this.tokens = tokens;
        this.whitespacesBefore = whitespacesBefore;
    }

    public static void Resolve(string text, List<Token> tokens)
        => new EmphasisResolver(text, tokens, CountWhitespaces(text)).Run();

    private void Run()
    {
        var parents = new Stack<Scope>();
        var scope = new Scope();

        for (var position = 0; position < tokens.Count; position++)
        {
            var token = tokens[position];

            switch (token)
            {
                case UnderscoreToken or DoubleUnderscoreToken:
                    Process(position, scope);
                    break;

                case LineFeedToken:
                    // перевод строки обрывает всё незакрытое и начинает новую область
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

    private void Process(int index, Scope scope)
    {
        var token = tokens[index];
        if (IsInsideTextWithDigits(token))
            return;

        var opened = OpenedOf(token, scope);
        if (CanClose(token) && opened.Count > 0 && TryClose(index, opened, scope))
            return;

        if (CanOpen(token))
            opened.Push(index);
    }

    private bool TryClose(int closeIndex, Stack<int> opened, Scope scope)
    {
        var openIndex = opened.Peek();
        if (!IsValidPair(tokens[openIndex], tokens[closeIndex]))
            return false;

        opened.Pop();

        var others = OtherOf(tokens[closeIndex], scope);
        if (others.Count > 0 && others.Peek() > openIndex)
        {
            while (others.Count > 0 && others.Peek() > openIndex)
                others.Pop();
            return true;
        }

        MarkPair(openIndex, closeIndex, scope);
        return true;
    }

    private void MarkPair(int openIndex, int closeIndex, Scope scope)
    {
        tokens[openIndex].Role = TokenRole.Opening;
        tokens[closeIndex].Role = TokenRole.Closing;

        if (tokens[openIndex] is DoubleUnderscoreToken)
            scope.StrongPairs.Add((openIndex, closeIndex));
        else
            DemoteStrongInside(openIndex, scope);
    }

    private void DemoteStrongInside(int emOpenIndex, Scope scope)
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

    private bool CanOpen(Token token)
        => token.End < text.Length && !char.IsWhiteSpace(text[token.End]);

    private bool CanClose(Token token)
        => token.Start > 0 && !char.IsWhiteSpace(text[token.Start - 1]);

    private bool IsInsideTextWithDigits(Token token)
    {
        var before = token.Start > 0 ? text[token.Start - 1] : '\0';
        var after = token.End < text.Length ? text[token.End] : '\0';
        return (char.IsDigit(before) && char.IsLetterOrDigit(after))
               || (char.IsDigit(after) && char.IsLetterOrDigit(before));
    }

    private bool IsValidPair(Token open, Token close)
    {
        var touchesWord = IsLetterOrDigitAt(open.Start - 1) || IsLetterOrDigitAt(close.End);
        return !touchesWord || whitespacesBefore[close.Start] - whitespacesBefore[open.End] == 0;
    }

    private bool IsLetterOrDigitAt(int index)
        => index >= 0 && index < text.Length && char.IsLetterOrDigit(text[index]);

    private static int[] CountWhitespaces(string text)
    {
        var counts = new int[text.Length + 1];
        for (var i = 0; i < text.Length; i++)
            counts[i + 1] = counts[i] + (char.IsWhiteSpace(text[i]) ? 1 : 0);
        return counts;
    }
}