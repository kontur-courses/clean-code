using Markdown.Tokens;

namespace Markdown;

public static class MarkdownParser
{
    public static LineToken ParseLine(string line)
    {
        var root = new LineToken(line);
        for (var i = 0; i < line.Length; i++)
        {
            if (line[i] == EscapeRules.Character)
            {
                if (i + 1 >= line.Length || TokenSelector.SelectLongestSuitableToken(line, i + 1) is null)
                    IncludeInTextToken(i, root);
                else
                {
                    root.NestedTokens.Add(new EscapeToken { FirstPosition = i });
                    IncludeInTextToken(i + 1, root);
                    i++;
                }
                continue;
            }
            
            var token = TokenSelector.SelectLongestSuitableToken(line, i);
            if (token is not null)
            {
                if (TryBuildToken(line, i, token, root))
                    i = root.NestedTokens[^1].LastPosition;
                else
                {
                    IncludeInTextToken(i, root, token.Opening.Length);
                    i += token.Opening.Length - 1;
                }
            }
            else
                IncludeInTextToken(i, root);
        }
        
        foreach (var nestingFilter in Nesting.Filters)
            nestingFilter.Filter(root);
        
        return root;
    }

    private static bool TryBuildToken(string line, int index, Token? token, Token? parentToken)
    {
        token = TryGetStartingToken(line, index, token, parentToken);
        if (token is null)
            return false;

        if (token is DoubleToken)
        {
            if (!TryEndDoubleToken(line, index, token))
                return false;
        }
        else
        {
            if (!TryEndSingleToken(line, index, token))
                return false;
        }
        
        parentToken?.NestedTokens.Add(token);
        return true;
    }
    
    private static void IncludeInTextToken(int index, Token parent, int length = 1)
    {
        if (parent.NestedTokens.Count > 0 && parent.NestedTokens[^1] is TextToken)
            parent.NestedTokens[^1].Length += length;
        else
            parent.NestedTokens.Add(new TextToken { FirstPosition = index, Length = length });
    }
    
    private static Token? TryGetStartingToken(string line, int index, Token? token, Token? parent)
    {
        if (!token?.CanStartsHere(line, index) ?? true)
            return null;
        
        token.FirstPosition = index;
        token.Parent = parent;
        return token;
    }

    private static bool TryEndSingleToken(string line, int index, Token token)
    {
        token.Length = line.Length - index;
        for (var i = index + token.Opening.Length; i < line.Length; i++)
        {
            var longestSuitableStart = TokenSelector.SelectLongestSuitableToken(line, i);
            var tokenToBuild = TryGetStartingToken(line, i, longestSuitableStart, token);
            if (TryBuildToken(line, i, tokenToBuild, token))
                i = token.NestedTokens[^1].LastPosition;
            else
                IncludeInTextToken(i, token);
        }

        return true;
    }
    
    private static bool TryEndDoubleToken(string line, int index, Token token)
    {
        var canBeCreated = true;
        for (var i = index + token.Opening.Length; line.IsInBound(i); i++)
        {
            var longestSuitableStart = TokenSelector.SelectLongestSuitableToken(line, i);
            if (longestSuitableStart is not null && token.GetType() != longestSuitableStart.GetType())
            {
                var tokenToBuild = TryGetStartingToken(line, i, longestSuitableStart, token);
                if (TryBuildToken(line, i, tokenToBuild, token))
                    i = token.NestedTokens[^1].LastPosition;
                else
                {
                    i += longestSuitableStart.Opening.Length - 1;
                    canBeCreated = false;
                }
                continue;
            }

            if (token.CanEndsHere(line, i))
            {
                token.Length = i + token.Ending.Length - token.FirstPosition;
                return canBeCreated;
            }
            
            IncludeInTextToken(i, token);
        }
        
        return false;
    }
}