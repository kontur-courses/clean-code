using Markdown.TagsType;

namespace Markdown.Parsers;

public static class GeneralParser
{
    public static Stack<Token> tagsStack = new();

    public delegate Token AddInsert(ITagsType tags, bool isClose);

    public delegate bool IsValidTag(string text, int index);

    public static Token AddInsideTag(ITagsType tag, bool isClose)
    {
        var result = GenerateTagToken(tag, isClose);
        if (!isClose && tagsStack.Count != 0 && tagsStack.Peek().Type?.MarkdownTag == tag.MarkdownTag)
            ClosePairTag(tagsStack.Pop());
        return result;
    }

    public static Token GenerateTagToken(ITagsType tag, bool isClose)
    {
        if (isClose)
        {
            var temp = new Token(tag.GetHtmlOpenTag, tag);
            tagsStack.Push(temp);
            return temp;
        }

        if (tagsStack.Count != 0 && tagsStack.Peek().Type?.MarkdownTag == tag.MarkdownTag)
            tag.HasPair = true;
        return new Token(tag.GetHtmlCloseTag, tag);
    }

    public static void ClosePairTag(Token token)
    {
        if (token.Type != null) token.Type.HasPair = true;
    }

    public static bool IsValidCloserTag(string text, int index)
    {
        if (index == 0)
            return false;
        if (index > 0 && text[index - 1] == ' ')
            return false;

        return true;
    }

    public static bool IsValidOpenTag(string text, int index)
    {
        if (index == text.Length - 1)
            return false;
        if (index < text.Length - 1 && text[index + 1] == ' ')
            return false;

        return true;
    }
}