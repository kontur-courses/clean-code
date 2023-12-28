namespace Markdown.Extensions;

public static class TokensExtensions
{
    public static LinkedList<IToken> HandleTokens(this LinkedList<IToken> tokens)
    {
        var stack = new Stack<MdTagToken>();

        for (var token = tokens.First; token != null; token = token.Next)
            token.HandleToken(stack);
        
        tokens.HandleSpecialTags();

        return tokens;
    }

    private static void HandleToken(this LinkedListNode<IToken> token, Stack<MdTagToken> openedTags)
    {
        switch (token.Value)
        {
            case MdEscapeToken escapeToken when token.Next != null:
                escapeToken.Escape(token.Next.Value);
                break;
            case MdNewlineToken or MdEndOfTextToken when openedTags.Count > 0:
                token.HandleNewlineOrEndOfTextToken(openedTags);
                break;
            case MdTagToken {Status: not Status.Broken}:
                token.HandleTagToken(openedTags);
                break;
        }
    }
    
    private static void HandleNewlineOrEndOfTextToken(this LinkedListNode<IToken> token, Stack<MdTagToken> openedTags)
    {
        while (openedTags.Count > 0)
        {
            var openedTag = openedTags.Pop();
            if (openedTag.Tag is HeaderTag)
            {
                openedTag.Status = Status.Opened;
                token.List!.AddBefore(token, new MdTagToken(new HeaderTag()) {Status = Status.Closed});
            }
            else
                openedTag.Status = Status.Broken;
        }
    }

    private static void HandleTagToken(this LinkedListNode<IToken> token, Stack<MdTagToken> openedTags)
    {
        var currentTag = (MdTagToken) token.Value;
        if (openedTags.Count == 0 && currentTag.AdjacentSymbols.Right == ' ')
        {
            currentTag.Status = Status.Broken;
            return;
        }
        if (openedTags.Count > 0 && openedTags.Peek().Tag == currentTag.Tag)
        {
            token.HandleMatchingTag(openedTags);
            return;
        }
        
        openedTags.Push(currentTag);
    }
    
    private static void HandleMatchingTag(this LinkedListNode<IToken> token, Stack<MdTagToken> openedTags)
    {
        var lastOpenedTag = openedTags.Peek();
        var closeTag = (MdTagToken)token.Value;
        if (closeTag.AdjacentSymbols.Left == ' ')
            closeTag.Status = Status.Broken;
        else if (token.Previous!.Value == lastOpenedTag || ((closeTag.IsInsideWord || lastOpenedTag.IsInsideWord) && token.List!.Find(lastOpenedTag)!.IsWhiteSpaceBetween(closeTag)))
        {
            openedTags.Pop().Status = Status.Broken;
            closeTag.Status = Status.Broken;
        }
        else
        {
            openedTags.Pop().Status = Status.Opened;
            closeTag.Status = Status.Closed;
        }
        
    }

    private static bool IsWhiteSpaceBetween(this LinkedListNode<IToken> token, MdTagToken pairTag)
    {
        for (var current = token.Next; current!.Value != pairTag; current = current.Next)
            if (current.Value.Value.Contains(' '))
                return true;

        return false;
    }
    
    private static void HandleSpecialTags(this LinkedList<IToken> tokens)
    {
        var tags = tokens.Where(x => x is MdTagToken tag && tag.Status != Status.Broken && tag.Tag.GetType() != typeof(HeaderTag)).Select(x => (MdTagToken)x).ToArray();
        for (var i = 0; i < tags.Length - 3; i++)
        {
            if (tags[i].Tag is BoldTag && tags[i + 1].Tag is ItalicTag && tags[i + 2].Tag is BoldTag &&
                tags[i + 3].Tag is ItalicTag)
            {
                foreach (var mdTagToken in tags[i..(i + 3)])
                {
                    mdTagToken.Status = Status.Broken;
                }
            }
            else if (tags[i].Tag is ItalicTag && tags[i + 1].Tag is BoldTag && tags[i + 2].Tag is BoldTag &&
                     tags[i + 3].Tag is ItalicTag)
            {
                tags[i + 1].Status = Status.Broken;
                tags[i + 2].Status = Status.Broken;
            }
        }
    }
}