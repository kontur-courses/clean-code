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
                        stack.Push(token);
                    }
                    else if (picked.Tag!.Status == TagStatus.Open && tokenTag.IsClosingFor(picked.Tag))
                    {
                        tokenTag.Status = TagStatus.Close;

                        if (tokenTag.Type is TagType.Bold or TagType.Italic)
                        {
                            var start = picked.Tag!.Context.Position;
                            var finish = tokenTag.Context.Position;

                            if (IsWhiteSpaceBetweenPositions(start, finish, tokenTag.Context.Text))
                            {
                                picked.Tag.Status = TagStatus.Broken;
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
                            if (innerToken.Tag!.Type == TagType.Newline)
                                break;

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

    private static bool IsWhiteSpaceBetweenPositions(int start, int end, string text)
    {
        for (var i = start; i < end; i++)
            if (char.IsWhiteSpace(text[i]))
                return true;

        return false;
    }
}