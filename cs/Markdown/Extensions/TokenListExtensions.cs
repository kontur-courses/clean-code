using Markdown.Contracts;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Extensions;

public static class TokenListExtensions
{
    public static void AcceptBrokenFilter(this IList<Token> tokens)
    {
        var prefilter = tokens.Where(token => token.Tag != null);

        foreach (var token in prefilter)
        {
            token.Tag!.ChangeStatusIfBroken();
        }
    }

    public static void MarkOpenCloseTags(this IList<Token> tokens)
    {
        var stack = new Stack<Token>();
        var prefilter = tokens
            .Where(token => token.Tag != null)
            .Where(token => token.Tag!.Status != TagStatus.Broken);

        foreach (var token in prefilter)
        {
            var tokenTag = token.Tag!;

            if (tokenTag.Status == TagStatus.Open)
            {
                stack.Push(token);
            }
            
            else if (tokenTag.Status == TagStatus.Close)
            {
                if (!stack.TryPeek(out var picked))
                {
                    stack.Push(token);
                }

                else if (picked.Tag!.Status == TagStatus.Open
                    && picked.Tag!.Info.GlobalMark == tokenTag.Info.GlobalMark)
                {
                    stack.Pop();
                }

                else
                {
                    stack.Push(token);
                }
            }
            
            else if (tokenTag.Status == TagStatus.Undefined)
            {
                if (!stack.TryPeek(out var picked))
                {
                    tokenTag.Status = TagStatus.Open;
                }
                
                else if (picked.Tag!.Status == TagStatus.Open 
                         && picked.Tag!.Info.GlobalMark == tokenTag.Info.GlobalMark)
                {
                    token.Tag!.Status = TagStatus.Close;
                    stack.Pop();
                }

                else
                {
                    token.Tag!.Status = TagStatus.Open;
                    stack.Push(token);
                }
            }
        }

        while (stack.Count != 0)
            stack.Pop().Tag!.Status = TagStatus.Broken;

        var insideItalic = false;

        foreach (var token in tokens
                     .Where(token => token.Tag != null)
                     .Where(token => token.Tag!.Status != TagStatus.Broken))
        {
            var tagToken = token.Tag!;

            if (tagToken.Type == TagType.Italic)
            {
                if (insideItalic)
                    insideItalic = false;
                else
                    insideItalic = true;
                
                continue;
            }

            if (insideItalic)
            {
                if (tagToken.Type == TagType.Bold)
                {
                    tagToken.Status = TagStatus.Broken;
                }
                
                else if (tagToken.Type == TagType.Italic)
                    insideItalic = false;
            }
        }
    }

    public static void SetTokenTypes(this IList<Token> tokens)
    {
        var prefilter = tokens
            .Where(token => token.Tag != null && token.Tag!.Status != TagStatus.Broken);

        foreach (var token in prefilter)
        {
            var tag = token.Tag!;

            if (tag.Type == TagType.Bold || tag.Type == TagType.Italic)
            {
                var offset = tag.Info.GlobalMark.Length;
                var prevChar = tag.PreviousChar;
                var nextChar = tag.NextChar;

                if (char.IsWhiteSpace(prevChar) && !char.IsWhiteSpace(nextChar))
                {
                    tag.Status = TagStatus.Open;
                }

                else if (!char.IsWhiteSpace(prevChar) && char.IsWhiteSpace(nextChar))
                {
                    tag.Status = TagStatus.Close;
                }
            }

            else if (tag.Type == TagType.Header)
            {
                tag.Status = TagStatus.Open;
            }
            
            else if (tag.Type == TagType.Newline)
            {
                tag.Status = TagStatus.Close;
            }
        }
    }
}