using Markdown.Tags.MdTags;
using Markdown.Tokens;

namespace Markdown;

internal static class MdTokensHandler
{
    public static IEnumerable<IToken> HandleTokens(this LinkedList<IToken> tokens)
    {
        var stack = new Stack<MdTagToken>();

        for (var token = tokens.First; token != null; token = token.Next)
            token.HandleToken(stack);
        tokens.HandleNestingAndIntersections();

        return tokens;
    }

    private static void HandleToken(this LinkedListNode<IToken> token, Stack<MdTagToken> openedTags)
    {
        switch (token.Value)
        {
            case MdEscapeToken escapeToken when token.Next != null:
                escapeToken.Escape(token.Next.Value);
                break;
            case MdNewlineToken or MdEndOfTextToken:
                token.HandleNewlineOrEndOfTextToken(openedTags);
                break;
            case MdTagToken {Status: not Status.Broken}:
                token.HandleTagToken(openedTags);
                break;
        }
    }
    
    private static void HandleNewlineOrEndOfTextToken(this LinkedListNode<IToken> endOfLineToken, Stack<MdTagToken> openedTags)
    {
        while (openedTags.Count > 0)
        {
            var openedTag = openedTags.Pop();
            if (openedTag.Tag is SingleTag singleTag && singleTag.IsOpenedCorrectly(openedTag.AdjacentSymbols))
            {
                openedTag.Status = Status.Opened;
                endOfLineToken.List!.AddBefore(endOfLineToken, new MdTagToken(openedTag.Tag) {Status = Status.Closed});

                if (singleTag.HtmlContainer == null) continue;
                
                var openedTokenNode = endOfLineToken.List!.Find(openedTag);
                if (openedTokenNode!.Previous?.Previous is not {Value: MdTagToken} prevTagToken 
                    || prevTagToken.Value is MdTagToken prevTag && prevTag.Tag != openedTag.Tag)
                    openedTokenNode.List!.AddBefore(openedTokenNode, new MdTextToken(singleTag.HtmlContainer));

                if (endOfLineToken.Next is not {Value: MdTagToken} 
                        || endOfLineToken.Next.Value is MdTagToken nextTag && nextTag.Tag != openedTag.Tag)
                    endOfLineToken.List!.AddBefore(endOfLineToken,
                        new MdTextToken(singleTag.HtmlContainer.Insert(1, "/")));
            }
            else
                openedTag.Status = Status.Broken;
        }
    }

    private static void HandleTagToken(this LinkedListNode<IToken> token, Stack<MdTagToken> openedTags)
    {
        var currentTag = (MdTagToken) token.Value;
        switch (openedTags.Count)
        {
            case 0 when !currentTag.Tag.IsOpenedCorrectly(currentTag.AdjacentSymbols):
                currentTag.Status = Status.Broken;
                return;
            case > 0 when currentTag.Tag is PairTag && openedTags.Peek().Tag == currentTag.Tag:
                token.HandleMatchingTag(openedTags);
                return;
            default:
                openedTags.Push(currentTag);
                break;
        }
    }
    
    private static void HandleMatchingTag(this LinkedListNode<IToken> token, Stack<MdTagToken> openedTags)
    {
        var lastOpenedTag = openedTags.Peek();
        var closeTag = (MdTagToken)token.Value;
        
        if (!closeTag.Tag.IsClosedCorrectly(closeTag.AdjacentSymbols))
            closeTag.Status = Status.Broken;
        else if (token.Previous!.Value == lastOpenedTag 
                 || ((closeTag.IsInsideWord || lastOpenedTag.IsInsideWord) && token.List!.Find(lastOpenedTag)!.IsWhiteSpaceBetween(closeTag)))
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
    
    private static void HandleNestingAndIntersections(this LinkedList<IToken> tokens)
    {
        var pairedTagTokens = tokens
            .Where(x => x is MdTagToken {Status: not Status.Broken, Tag: PairTag})
            .Select(x => (MdTagToken)x)
            .ToArray();
        
        for (var i = 0; i < pairedTagTokens.Length - 1; i++)
        {
            var currentToken = pairedTagTokens[i];
            var currentTag = (PairTag)pairedTagTokens[i].Tag;
            var nextToken = pairedTagTokens[i + 1];
            
            if (currentToken.Status != Status.Opened || currentTag == nextToken.Tag) continue;
            
            if (nextToken.Status == Status.Closed)
            {
                tokens.BrokePaired(currentToken);
                tokens.BrokePaired(nextToken);
            }
            else if (!currentTag.CanContain(nextToken.Tag))
                tokens.BrokePaired(nextToken);
        }
    }

    private static void BrokePaired(this LinkedList<IToken> tokens, MdTagToken pairedTag)
    {
        if (pairedTag.Status == Status.Opened)
        {
            for (var token = tokens.First; token != null; token = token.Next)
                if (token.Value is MdTagToken {Status: Status.Closed} tagToken && tagToken.Tag == pairedTag.Tag)
                    tagToken.Status = Status.Broken;
        }
        else if (pairedTag.Status == Status.Closed)
        {
            for (var token = tokens.First; token != null; token = token.Previous)
                if (token.Value is MdTagToken {Status: Status.Opened} tagToken && tagToken.Tag == pairedTag.Tag)
                    tagToken.Status = Status.Broken;
        }
        
        pairedTag.Status = Status.Broken;
    }
}