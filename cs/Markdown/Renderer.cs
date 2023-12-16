namespace Markdown;

public class Renderer
{
    private readonly Dictionary<string, string> htmlTagDict = Tags.HtmlTagDict;

    private readonly Dictionary<string, Tag> tagDict = new()
    {
        { "__", new Tag(true, TagType.Bold, new[] { "_" }) },
        { "_", new Tag(true, TagType.Italic) },
        { "# ", new Tag(false, TagType.Heading) },
        { "\n", new Tag(false, TagType.LineBreaker) },
        { "\r\n", new Tag(false, TagType.LineBreaker) }
    };

    private bool hasDigitText;
    private bool hasSpacesBetweenTags;
    private Stack<Token>? stack;

    public List<Token> HandleTokens(List<Token> tokenList)
    {
        stack = new Stack<Token>();
        if (tokenList.All(x => x.Status != TokenStatus.Text)) return tokenList;
        foreach (var token in tokenList)
        {
            if (token.Status == TokenStatus.Text)
            {
                if (!hasSpacesBetweenTags) hasSpacesBetweenTags = token.isSpaces;
                if (!hasDigitText) hasDigitText = token.isDigitText;
                continue;
            }

            if (IsEscapeOrUnpairedTag(token))
            {
                token.Content = htmlTagDict[token.Content];
                continue;
            }

            HandlePairedTag(token);
        }

        return tokenList;
    }

    private void HandlePairedTag(Token token)
    {
        if (!tagDict[token.Content].IsOpen)
        {
            OpenPairedTag(token, stack);
            return;
        }

        stack.TryPeek(out var tokenPeek);
        if (tokenPeek == null) return;
        if (tokenPeek.Content == token.Content &&
            (!hasSpacesBetweenTags || IsTagWordBoundary(token, tokenPeek)))
            ClosePairedTag(token);
        else if (tokenPeek.Content != token.Content) ResetStack(token);
    }

    private bool IsEscapeOrUnpairedTag(Token token)
    {
        return token.Status == TokenStatus.EscapeTag || !tagDict[token.Content].IsPaired;
    }

    private void OpenPairedTag(Token token, Stack<Token> stack)
    {
        tagDict[token.Content].IsOpen = true;
        stack.Push(token);
    }

    private bool CheckBlockedTagsBeforeToken(Token token)
    {
        stack.TryPeek(out var prevtokenPeek);
        return prevtokenPeek != null && tagDict[token.Content].BlockTags.Contains(prevtokenPeek.Content);
    }

    private void ClosePairedTag(Token token)
    {
        var tokenPeek = stack.Pop();
        stack.TryPeek(out var prevtokenPeek);
        if (CheckBlockedTagsBeforeToken(token) || hasDigitText)
        {
            hasDigitText = false;
            tagDict[token.Content].IsOpen = false;
            return;
        }

        tagDict[token.Content].IsOpen = false;
        tokenPeek.Content = htmlTagDict[token.Content];
        token.Content = $"</{tokenPeek.Content[1..]}";
        hasSpacesBetweenTags = false;
    }

    private void ResetStack(Token token)
    {
        stack.Clear();
        stack.Push(token);
    }

    private static bool IsTagWordBoundary(Token token, Token tokenPeek)
    {
        return token.Last is "" or " " && tokenPeek.Last != " " && tokenPeek.Prev is "" or " " && token.Prev != " ";
    }
}