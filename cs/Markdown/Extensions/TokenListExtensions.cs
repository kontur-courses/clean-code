using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Extensions;

public static class TokenListExtensions
{
    public static void DetermineTagStatuses(this IList<Token> tokens)
    {
        var stack = new Stack<Token>();

        foreach (var token in tokens)
        {
            var tokenTag = token.Tag!;

            tokenTag.SetTagStatus();
            tokenTag.ChangeStatusIfBroken();

            switch (tokenTag.Status)
            {
                case TagStatus.Broken:
                    continue;

                case TagStatus.Open:
                    stack.Push(token);
                    break;

                case TagStatus.Close:
                {
                    if (!stack.TryPeek(out var pickedToken))
                        stack.Push(token);
                    else if (pickedToken.Tag!.Status == TagStatus.Open && pickedToken.Tag.IsClosingFor(tokenTag))
                        stack.Pop();
                    else
                        stack.Push(token);

                    break;
                }

                case TagStatus.Undefined:
                {
                    if (!stack.TryPeek(out var picked))
                    {
                        tokenTag.Status = TagStatus.Open;
                    }
                    else if (picked.Tag!.Status == TagStatus.Open && tokenTag.IsClosingFor(picked.Tag))
                    {
                        tokenTag.Status = TagStatus.Close;
                        stack.Pop();
                    }
                    else if (tokenTag.Type == TagType.Newline)
                    {
                        foreach (var innerToken in stack)
                        {
                            if (innerToken.Tag!.Type == TagType.Newline)
                            {
                                tokenTag.Status = TagStatus.Open;
                                break;
                            }

                            if (!tokenTag.IsClosingFor(innerToken.Tag!))
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
                }

                default:
                    throw new Exception("Unknown tag status");
            }
        }

        while (stack.Count != 0)
        {
            var popped = stack.Pop().Tag!;

            if (popped.Type is TagType.Header or TagType.Newline)
                continue;

            popped.Status = TagStatus.Broken;
        }
    }

    public static void FilterIntersections(this IList<Token> tokens)
    {
        var insideItalic = false;

        foreach (var token in tokens.Where(token => token.Tag!.Status != TagStatus.Broken))
        {
            var tagToken = token.Tag!;

            if (tagToken.Type == TagType.Italic)
                insideItalic = !insideItalic;

            if (insideItalic && tagToken.Type == TagType.Bold)
                tagToken.Status = TagStatus.Broken;
        }
    }

    public static void FilterEmptyTags(this IList<Token> tokens)
    {
        for (var i = 0; i < tokens.Count - 1; i++)
        {
            var current = tokens[i].Tag;
            var next = tokens[i + 1].Tag;

            if (current == null || next == null)
                continue;

            if (current.Type != next.Type)
                continue;

            current.Status = TagStatus.Broken;
            next.Status = TagStatus.Broken;
        }
    }
}