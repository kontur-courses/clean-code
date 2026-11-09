using Markdown.Tokens;

namespace Markdown;

internal sealed class EmphasisResolver
{
    private readonly string text;
    private readonly List<TextToken> tokens;
    private readonly int[] whitespacesBefore;

    private readonly Stack<int> openedEm = new();
    private readonly Stack<int> openedStrong = new();
    private readonly List<(int Open, int Close)> strongPairs = new();

    private EmphasisResolver(string text, List<TextToken> tokens, int[] whitespacesBefore)
    {
        this.text = text;
        this.tokens = tokens;
        this.whitespacesBefore = whitespacesBefore;
    }

    public static void Resolve(string text, List<TextToken> tokens)
        => new EmphasisResolver(text, tokens, CountWhitespaces(text)).Run(0);
    
    private int NewRun(int start)
        => new EmphasisResolver(text, tokens, whitespacesBefore).Run(start);
        

    private int Run(int start)
    {
        var position = start;

        while (position < tokens.Count)
        {
            var token = tokens[position];
            if (token is UnderscoreToken || token is DoubleUnderscoreToken)
                Process(position);
            else if (token is LineFeedToken)
            {
                if (token.Start + 1 <= text.Length)
                    NewRun(position + 1);
                break;
            }
            else if (token is LinkToken || token is HeaderToken)
            {
                if (token.Role == TokenRole.Opening)
                {
                    position = NewRun(position + 1);
                }
                if (token.Role == TokenRole.Closing)
                {
                    return position;
                }
            }
            position++;
        }

        return position;
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

    private bool TryClose(int closeIndex, Stack<int> opened)
    {
        var openIndex = opened.Peek();
        if (!IsValidPair(tokens[openIndex], tokens[closeIndex]))
            return false;

        opened.Pop();

        var others = OtherOf(tokens[closeIndex]);
        if (others.Count > 0 && others.Peek() > openIndex)
        {
            while (others.Count > 0 && others.Peek() > openIndex)
                others.Pop();
            return true;
        }

        MarkPair(openIndex, closeIndex);
        return true;
    }

    private void MarkPair(int openIndex, int closeIndex)
    {
        tokens[openIndex].Role = TokenRole.Opening;
        tokens[closeIndex].Role = TokenRole.Closing;
        
        if (tokens[openIndex] is DoubleUnderscoreToken)
        {
            strongPairs.Add((openIndex, closeIndex));
        }
        else
        {
            DemoteStrongInside(openIndex);
        }
            
    }

    private void DemoteStrongInside(int emOpenIndex)
    {
        while (strongPairs.Count > 0 && strongPairs[^1].Open > emOpenIndex)
        {
            var (open, close) = strongPairs[^1];
            tokens[open].Role = TokenRole.Literal;
            tokens[close].Role = TokenRole.Literal;
            strongPairs.RemoveAt(strongPairs.Count - 1);
        }
    }

    private Stack<int> OpenedOf(TextToken textToken)
        => textToken is UnderscoreToken ? openedEm : openedStrong;

    private Stack<int> OtherOf(TextToken textToken)
        => textToken is UnderscoreToken ? openedStrong : openedEm;

    private bool CanOpen(TextToken textToken)
        => textToken.End < text.Length && !char.IsWhiteSpace(text[textToken.End]);

    private bool CanClose(TextToken textToken)
        => textToken.Start > 0 && !char.IsWhiteSpace(text[textToken.Start - 1]);

    private bool IsInsideTextWithDigits(TextToken textToken)
    {
        var before = textToken.Start > 0 ? text[textToken.Start - 1] : '\0';
        var after = textToken.End < text.Length ? text[textToken.End] : '\0';
        return (char.IsDigit(before) && char.IsLetterOrDigit(after))
               || (char.IsDigit(after) && char.IsLetterOrDigit(before));
    }

    private bool IsValidPair(TextToken open, TextToken close)
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