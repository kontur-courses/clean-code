using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown;

internal class NestingHandler
{
    public void HandleNesting(LinkedList<IToken> tokens)
    {
        var boundaryTagTokens = tokens
            .Where(x => x is TagToken { Status: not TagStatus.Broken, Tag: BoundaryTag })
            .Select(x => (TagToken)x).ToList();

        for (var i = 0; i < boundaryTagTokens.Count - 1; i++)
        {
            var currentToken = boundaryTagTokens[i];
            var currentTag = (BoundaryTag)currentToken.Tag;
            var nextToken = boundaryTagTokens[i + 1];
            var nextTag = (BoundaryTag)nextToken.Tag;

            if (currentToken.Status != TagStatus.Opened || currentTag == nextToken.Tag) 
                continue;

            if (nextToken.Status == TagStatus.Closed)
            {
                BreakTags(tokens, currentToken);
                BreakTags(tokens, nextToken);
            }
            else if (!nextTag.CanBeInnerTag(currentTag))
                BreakTags(tokens, nextToken);
        }
    }

    private void BreakTags(LinkedList<IToken> tokens, TagToken boundaryTag)
    {
        switch (boundaryTag)
        {
            case { Status: TagStatus.Opened }:
                BreakClosingTags(tokens, boundaryTag);
                break;
            case { Status: TagStatus.Closed }:
                BreakOpeningTags(tokens, boundaryTag);
                break;
        }

        boundaryTag.Status = TagStatus.Broken;
    }

    private void BreakClosingTags(LinkedList<IToken> tokens, TagToken openedTag)
    {
        var closingTag = tokens.Where(x =>
            x is TagToken { Status: TagStatus.Closed } tagToken &&
            tagToken.Tag == openedTag.Tag);
        
        foreach (var token in closingTag)
            ((TagToken)token).Status = TagStatus.Broken;
    }

    private void BreakOpeningTags(LinkedList<IToken> tokens, TagToken closedTag)
    {
        var openingTags = tokens.Where(x =>
            x is TagToken { Status: TagStatus.Opened } tagToken
            && tagToken.Tag == closedTag.Tag);
        
        foreach (var token in openingTags)
            ((TagToken)token).Status = TagStatus.Broken;
    }
}