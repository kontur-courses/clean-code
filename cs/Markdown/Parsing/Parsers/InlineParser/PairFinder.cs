namespace Markdown.Parsing.Parsers.InlineParser;

public class PairFinder
{
    public HashSet<(int Index, bool IsOpening)> FindValidPairs(List<Token> tokens, string text)
    {
        var allPairs = FindAllPairs(tokens, text);
        return FilterPairsByNesting(allPairs, tokens);
    }

    private List<(int Open, int Close)> FindAllPairs(List<Token> tokens, string text)
    {
        var allPairs = new List<(int Open, int Close)>();
        var stack = new Stack<int>();

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (token.Type == TokenType.Text) continue;

            var canBeOpening = IsNotSpaceAfter(text, token);
            var canBeClosing = IsNotSpaceBefore(text, token);

            if (canBeClosing && stack.Count > 0)
            {
                if (tokens[stack.Peek()].Type == token.Type)
                {
                    var openIndex = stack.Pop();
                    if (HasValidContent(openIndex, i, token, tokens, text))
                    {
                        allPairs.Add((openIndex, i));
                    }
                }
                else
                {
                    var isTriple = stack.Count > 0 &&
                                   stack.Peek() == i - 1 &&
                                   tokens[stack.Peek()].Type != token.Type;

                    if (isTriple)
                    {
                        if (canBeOpening) stack.Push(i);
                    }
                    else
                    {
                        while (stack.Count > 0 && tokens[stack.Peek()].Type != token.Type)
                            stack.Pop();
                        if (stack.Count > 0) stack.Pop();
                    }
                }
            }
            else if (canBeOpening)
            {
                stack.Push(i);
            }
        }

        return allPairs;
    }

    private HashSet<(int Index, bool IsOpening)> FilterPairsByNesting(
        List<(int Open, int Close)> allPairs,
        List<Token> tokens)
    {
        var validPairs = new HashSet<(int, bool)>();
        var italicIntervals = allPairs
            .Where(p => tokens[p.Open].Type == TokenType.Italic)
            .OrderBy(p => p.Open)
            .ToArray();

        var prefixMaxClose = new int[italicIntervals.Length];
        for (var i = 0; i < italicIntervals.Length; i++)
            prefixMaxClose[i] = Math.Max(i == 0 ? -1 : prefixMaxClose[i - 1], italicIntervals[i].Close);

        foreach (var (openIndex, closeIndex) in allPairs)
        {
            if (tokens[openIndex].Type == TokenType.Bold)
            {
                var lo = 0;
                var hi = italicIntervals.Length - 1;
                var candidate = -1;
                while (lo <= hi)
                {
                    var mid = lo + (hi - lo) / 2;
                    if (italicIntervals[mid].Open < openIndex)
                    {
                        candidate = mid;
                        lo = mid + 1;
                    }
                    else hi = mid - 1;
                }

                if (candidate >= 0 && prefixMaxClose[candidate] > closeIndex)
                    continue;
            }

            validPairs.Add((openIndex, true));
            validPairs.Add((closeIndex, false));
        }

        return validPairs;
    }

    private bool HasValidContent(int openIndex, int closeIndex, Token closeToken, List<Token> tokens, string text)
    {
        var hasContent = false;
        var hasSpace = false;

        for (var k = openIndex + 1; k < closeIndex; k++)
        {
            if (tokens[k].Type == TokenType.Text)
            {
                if (tokens[k].Value.Trim().Length > 0) hasContent = true;
                if (tokens[k].Value.Contains(' ')) hasSpace = true;
            }
            else
            {
                hasContent = true;
            }
        }

        if (!hasContent) return false;

        if (closeToken.Type == TokenType.Italic && hasSpace)
        {
            var openPos = tokens[openIndex].Position;
            var closePos = closeToken.Position;

            var openIsBoundary = openPos == 0 || !char.IsLetterOrDigit(text[openPos - 1]);
            var closeIsBoundary = closePos + closeToken.Value.Length >= text.Length ||
                                  !char.IsLetterOrDigit(text[closePos + closeToken.Value.Length]);

            return openIsBoundary && closeIsBoundary;
        }

        return true;
    }

    private bool IsNotSpaceAfter(string text, Token token)
    {
        var index = token.Position + token.Value.Length;
        return index >= text.Length || !char.IsWhiteSpace(text[index]);
    }

    private bool IsNotSpaceBefore(string text, Token token)
    {
        return token.Position == 0 || !char.IsWhiteSpace(text[token.Position - 1]);
    }
}