using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenHandlers;

internal class NewlineTokenHandler : ITokenHandler
{
    public void Handle(LinkedListNode<IToken> node, Stack<TagToken> stack)
    {
        if (node.Value is NewlineToken or EOFToken)
        {
            HandleNewlineToken(node, stack);
        }
    }

    private void HandleNewlineToken(LinkedListNode<IToken> node, Stack<TagToken> stack)
    {
        while (stack.Count > 0)
        {
            var openedTag = stack.Pop();

            if (IsValidBlockTag(openedTag))
                ProcessBlockTag(openedTag, node);
            else
                openedTag.Status = TagStatus.Broken;
        }
    }

    private bool IsValidBlockTag(TagToken tagToken)
    {
        return tagToken.Tag is BlockTag blockTag &&
               blockTag.CanBeOpened(tagToken.Left, tagToken.Right);
    }

    private void ProcessBlockTag(TagToken openedTag,
        LinkedListNode<IToken> currentNode)
    {
        openedTag.Status = TagStatus.Opened;
        InsertClosingTag(openedTag, currentNode);

        if (ShouldProcessHtmlTags(openedTag))
            InsertHtmlTagsForBlock((BlockTag)openedTag.Tag, openedTag, currentNode);
    }

    private void InsertClosingTag(TagToken openedTag, LinkedListNode<IToken> currentNode)
    {
        currentNode.List.AddBefore(currentNode,
            new TagToken(openedTag.Tag, openedTag.Left, openedTag.Right)
            {
                Status = TagStatus.Closed
            });
    }

    private void InsertHtmlTagsForBlock(BlockTag blockTag, TagToken openedTag,
        LinkedListNode<IToken> currentNode)
    {
        var openedTokenNode = currentNode.List!.Find(openedTag);
        
        var previousTag = openedTokenNode.Previous?.Previous?.Value as TagToken;
        if (previousTag?.Tag != openedTag.Tag)
            openedTokenNode.List.AddBefore(openedTokenNode, new TextToken(blockTag.HtmlBlockOpenTag));
        
        
        var nextTag = currentNode.Next?.Value as TagToken;
        if (nextTag?.Tag != openedTag.Tag)
            currentNode.List.AddBefore(currentNode, new TextToken(blockTag.HtmlBlockCloseTag));
    }

    private bool ShouldProcessHtmlTags(TagToken openedTag)
    {
        return openedTag.Tag is BlockTag blockTag &&
               !(string.IsNullOrEmpty(blockTag.HtmlBlockOpenTag) &&
                 string.IsNullOrEmpty(blockTag.HtmlBlockCloseTag));
    }
}