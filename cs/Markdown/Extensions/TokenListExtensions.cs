using Markdown.Enums;
using Markdown.Interfaces;
using Markdown.Tokens;

namespace Markdown.Extensions;

public static class TokenListExtensions
{
    public static void SetTagStatuses(this IList<MarkdownToken> tokens)
    {
        var stack = new Stack<MarkdownToken>();

        foreach (var token in tokens)
        {
            var tokenTag = token.AssociatedTag!;

            if (tokenTag.Status != TagStatus.Broken)
            {
                tokenTag.SetTagStatus();
                tokenTag.ChangeStatusIfBroken();
            }

            switch (tokenTag.Status)
            {
                case TagStatus.Broken:
                    continue;

                case TagStatus.Open:
                    stack.Push(token);
                    break;

                case TagStatus.Close:
                    if (!stack.TryPeek(out var pickedToken))
                    {
                        stack.Push(token);
                    }
                    else if (pickedToken.AssociatedTag!.Status == TagStatus.Open &&
                             pickedToken.AssociatedTag.IsClosingFor(tokenTag))
                    {
                        stack.Pop();
                    }
                    else
                    {
                        stack.Push(token);
                    }

                    break;

                case TagStatus.Undefined:
                    if (tokenTag.Type == TagType.ListItem)
                    {
                        tokenTag.Status = TagStatus.Open;
                        stack.Push(token);
                        break;
                    }

                    if (!stack.TryPeek(out var picked))
                    {
                        tokenTag.Status = TagStatus.Open;
                        stack.Push(token);
                    }
                    else if (picked.AssociatedTag!.Status == TagStatus.Open &&
                             tokenTag.IsClosingFor(picked.AssociatedTag))
                    {
                        tokenTag.Status = TagStatus.Close;

                        if (tokenTag.Type is TagType.Bold or TagType.Italic)
                        {
                            var start = picked.AssociatedTag!.MarkdownContext.Position;
                            var finish = tokenTag.MarkdownContext.Position;

                            if (IsWhiteSpaceBetweenPositions(start, finish, tokenTag.MarkdownContext.Text))
                            {
                                picked.AssociatedTag.Status = TagStatus.Broken;
                                tokenTag.Status = TagStatus.Broken;
                            }
                        }

                        stack.Pop();
                    }
                    else if (tokenTag.Type == TagType.Newline)
                    {
                        tokenTag.Status = TagStatus.Open;

                        foreach (var innerToken in stack)
                        {
                            if (innerToken.AssociatedTag!.Type == TagType.Newline)
                                break;

                            if (!tokenTag.IsClosingFor(innerToken.AssociatedTag!))
                                continue;

                            tokenTag.Status = TagStatus.Close;
                            break;
                        }

                        stack.Push(token);
                    }
                    else
                    {
                        tokenTag.Status = TagStatus.Open;
                        stack.Push(token);
                    }

                    break;

                default:
                    throw new Exception("Неизвестный статус тега");
            }
        }

        CleanUpStack(stack);
    }

    public static void RemoveIntersectingTags(this IList<MarkdownToken> tokens)
    {
        var insideItalic = false;

        foreach (var token in tokens.Where(token => token.AssociatedTag!.Status != TagStatus.Broken))
        {
            var tagToken = token.AssociatedTag!;

            UpdateInsideItalicFlag(tagToken, ref insideItalic);
            HandleIntersection(tagToken, ref insideItalic);
        }
    }

    private static void UpdateInsideItalicFlag(IMarkdownTag tagToken, ref bool insideItalic)
    {
        if (tagToken.Type == TagType.Italic)
            insideItalic = !insideItalic;
    }

    private static void HandleIntersection(IMarkdownTag tagToken, ref bool insideItalic)
    {
        if (insideItalic && tagToken.Type == TagType.Bold)
            tagToken.Status = TagStatus.Broken;
    }

    private static void CleanUpStack(Stack<MarkdownToken> stack)
    {
        while (stack.Count != 0)
        {
            var popped = stack.Pop().AssociatedTag!;

            if (popped.Type is TagType.Header or TagType.Newline)
                continue;

            popped.Status = TagStatus.Broken;
        }
    }

    private static bool IsWhiteSpaceBetweenPositions(int start, int end, string text)
    {
        for (var i = start; i < end; i++)
            if (char.IsWhiteSpace(text[i]))
                return true;

        return false;
    }
}