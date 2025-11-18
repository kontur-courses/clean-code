using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenHandlers;

internal class TagTokenHandler : ITokenHandler
{
    public void Handle(LinkedListNode<IToken> node, Stack<TagToken> stack)
    {
        if (node.Value is TagToken { Status: not TagStatus.Broken } currentTag)
        {
            HandleTagToken(node, currentTag, stack);
        }
    }

    private void HandleTagToken(LinkedListNode<IToken> node,
        TagToken currentTag, Stack<TagToken> stack)
    {
        switch (stack.Count)
        {
            case 0 when !currentTag.Tag.CanBeOpened(currentTag.Left, currentTag.Right):
                currentTag.Status = TagStatus.Broken;
                return;
            case > 0 when currentTag.Tag is BoundaryTag && stack.Peek().Tag == currentTag.Tag:
                HandleMatchingTag(node, currentTag, stack);
                return;
            default:
                stack.Push(currentTag);
                break;
        }
    }
    
    private void HandleMatchingTag(LinkedListNode<IToken> node, TagToken closeTag, Stack<TagToken> stack)
    {
        var lastOpenedTag = stack.Peek();

        if (!closeTag.Tag.CanBeClosed(closeTag.Left, closeTag.Right))
            closeTag.Status = TagStatus.Broken;
        
        else if (IsInvalidTagPair(node, lastOpenedTag, closeTag))
        {
            stack.Pop().Status = TagStatus.Broken;
            closeTag.Status = TagStatus.Broken;
        }
        else
        {
            stack.Pop().Status = TagStatus.Opened;
            closeTag.Status = TagStatus.Closed;
        }
    }

    private bool IsInvalidTagPair(LinkedListNode<IToken> node, TagToken openedTag, TagToken closeTag)
    {
        return node.Previous.Value == openedTag 
               || ((closeTag.IsInWord || openedTag.IsInWord) && node.List.Find(openedTag).HasWhiteSpaceBetween(closeTag));
    }
}
