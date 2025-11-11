using Markdown.Dto;

namespace Markdown.TagUtils;

public static class CloseContext
{
    public static bool IsInvalidUnderScoreCloseContext(Tag parent, Token token, int markerLength)
    {
        return IsInvalidCloseContext(parent, token, markerLength) || parent.HasOnlyDigits;
    }

    public static bool IsInvalidLinkCloseContext(Tag parent, Token token, int markerLength)
    {
        if (IsInvalidCloseContext(parent, token, markerLength)) return true;
        
        var content = parent.Content.ToString();
        var closeBracketIndex = content.IndexOf(']');
        var openParenIndex = content.IndexOf('(');
        
        const int startCloseBracketIndex = 0;
        var endCloseParenIndex = content.Length;

        if (closeBracketIndex == -1 || openParenIndex == -1)
        {
            return true;
        }

        if (closeBracketIndex - startCloseBracketIndex == 1)
        {
            return true;
        }

        if (endCloseParenIndex - openParenIndex == 1)
        {
            return true;
        }
        
        return openParenIndex - closeBracketIndex != 1;
    }

    private static bool IsInvalidCloseContext(Tag? parent, Token token, int markerLength)
    {
        if (parent == null)
        {
            return true;
        }

        if (parent.Token.Type != token.Type)
        {
            return true;
        }

        if (parent.Token.Position == TokenPosition.Inside && parent.ContainsSpace)
        {
            return true;
        }

        var contentLength = parent.Content.Length - markerLength;
        return contentLength <= 0;
    }
}