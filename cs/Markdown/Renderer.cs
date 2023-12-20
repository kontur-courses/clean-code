using System.Diagnostics;
using System.Text;
using Markdown.Tags;

namespace Markdown;

public class Renderer
{
    private bool hasDigitText;
    private bool hasSpacesBetweenTags;
    private bool textBetweenTags;
    private Tag PreviousTag;
    private Stack<Tag>? stack;

    public List<Token> HandleTokens(List<Token> tokenList)
    {
        var closingSomeTags = string.Empty;
        stack = new Stack<Tag>();
        foreach (var token in tokenList)
        {
            switch (token.Type)
            {
                case TokenType.Text:
                    break;
                case TokenType.Tag:
                    if (token.Tag.IsPaired)
                    {
                        HandlePairedTag(token.Tag);
                    }
                    else
                    {
                        if (token.Tag.Status != TagStatus.Block)
                        {
                            token.Tag.Content = token.Tag.ConvertTo;
                            closingSomeTags = token.Tag.ClosingTag + closingSomeTags;
                        }
                    }
                    break;
                case TokenType.Escape:
                    token.Content = "";
                    break;
                case TokenType.LineBreaker:
                    token.Content = closingSomeTags + token.Content;
                    closingSomeTags = string.Empty;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        return tokenList;
    }

    private void HandlePairedTag(Tag tag)
    {
        if (tag.Status == TagStatus.Block) return;
        if(tag.Status == TagStatus.Closing && PreviousTag == null) return;
        if (tag.Status == TagStatus.Opening || PreviousTag == null || 
            (PreviousTag.TokenType != tag.TokenType && PreviousTag.Status != TagStatus.Closing))
        {
            PreviousTag = tag;
            OpenPairedTag(tag, stack);
            return;
        }
        stack.TryPeek(out var tokenPeek);
        if (tokenPeek == null) return;
        if (tokenPeek.TokenType == tag.TokenType)
            ClosePairedTag(tag);
    }

    private void OpenPairedTag(Tag tag, Stack<Tag> stack)
    {
        tag.Status = TagStatus.Opening;
        stack.Push(tag);
    }

    private void ClosePairedTag(Tag tag)
    {
        var tokenPeek = stack.Pop();
        tag.Status = TagStatus.Closing;
        tokenPeek.Content = tag.Content = tag.ConvertTo;
        tag.Content = tag.ClosingTag;
        PreviousTag = tag;
    }
    
    
}
