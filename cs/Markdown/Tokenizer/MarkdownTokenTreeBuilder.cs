using Markdown.Tags;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer;

public static class MarkdownTokenTreeBuilder
{
    /// <summary>
    /// Собирает дерево токенов, исходя из тегов в тексте
    /// </summary>
    /// <param name="availableTokens">Список валидных тегов в тексте</param>
    /// <param name="content">Исходная строка</param>
    /// <returns>Дерево токенов</returns>
    public static List<IToken> BuildTokenTree(List<TagToken> availableTokens, string content)
    {
        var tree = new List<IToken>();
        var tagStack = new Stack<TagToken>();
        var lastTagEnd = 0;

        if (availableTokens.Count == 0)
        {
            tree.Add(new TextToken(content));
            return tree;
        }

        foreach (var tagToken in availableTokens)
        {
            if (tagStack.Count == 0)
                lastTagEnd = HandleFirstTag(tagToken, content, tree, tagStack, lastTagEnd);
            else if (tagToken.Tag.SelfClosing)
                lastTagEnd = HandleSelfClosingTag(tagToken, content, tagStack, lastTagEnd);
            else if (tagToken.Tag.Matches(tagStack.Peek().Tag))
                lastTagEnd = HandleClosingTag(tagToken, content, tree, tagStack, lastTagEnd);
            else
                lastTagEnd = HandleNestedTag(tagToken, content, tagStack, lastTagEnd);
        }

        if (lastTagEnd < content.Length)
            tree.Add(new TextToken(content[lastTagEnd..]));

        return tree;
    }

    private static int HandleFirstTag(TagToken tag, string content, List<IToken> tree, Stack<TagToken> tagStack,
        int lastTagEnd)
    {
        if (tag.Position > lastTagEnd)
        {
            var textToken = new TextToken(content.Substring(lastTagEnd, tag.Position - lastTagEnd));
            tree.Add(textToken);
        }


        if (tag.Tag.SelfClosing)
            tree.Add(tag);
        else
            tagStack.Push(tag);

        return tag.Position + tag.Tag.MdTag.Length;
    }

    private static int HandleSelfClosingTag(TagToken tag, string content, Stack<TagToken> tagStack, int lastTagEnd)
    {
        if (tag.Position > lastTagEnd)
        {
            var textToken = new TextToken(content.Substring(lastTagEnd, tag.Position - lastTagEnd));
            tagStack.Peek().Children.Add(textToken);
        }

        tagStack.Peek().Children.Add(tag);
        return tag.Position + tag.Tag.MdTag.Length;
    }

    private static int HandleClosingTag(TagToken tag, string content, List<IToken> tree, Stack<TagToken> tagStack,
        int lastTagEnd)
    {
        var tagToken = tagStack.Pop();

        if (tag.Position > lastTagEnd)
        {
            var textToken = new TextToken(content.Substring(lastTagEnd, tag.Position - lastTagEnd));
            tagToken.Children.Add(textToken);
        }

        // Handle image tag
        if (tagToken.Tag is ImageTag imageTag)
        {
            var imageToken = new TagToken(imageTag)
            {
                Position = tagToken.Position,
                Attributes = ImageTag.GetHtmlTadAttributes(
                    content.Substring(lastTagEnd - tagToken.Tag.MdTag.Length,
                        tag.Position - lastTagEnd + tagToken.Tag.MdTag.Length + 1))
            };
            tree.Add(imageToken);
            return tag.Position + tag.Tag.MdClosingTag.Length;
        }

        if (tagStack.Count == 0)
            tree.Add(tagToken);
        else
            tagStack.Peek().Children.Add(tagToken);

        var offset = tag.Tag.SelfClosing ? tag.Tag.MdTag.Length : tag.Tag.MdClosingTag.Length;
        return tag.Position + offset;
    }

    private static int HandleNestedTag(TagToken tag, string content, Stack<TagToken> tagStack, int lastTagEnd)
    {
        var lastTagInStack = tagStack.Peek();

        var length = tag.Position - lastTagEnd;
        if (length > 0)
        {
            var text = content.Substring(lastTagEnd, length);
            lastTagInStack.Children.Add(new TextToken(text));
        }
        tagStack.Push(tag);
        return tag.Position + tag.Tag.MdTag.Length;
    }
}