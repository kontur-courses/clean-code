using System.Text;
using Markdown.Tags;
using Markdown.Token;

namespace Markdown.Parse;

public class MarkdownMarkupParser : IMarkupParser
{
    private const char EscapeChar = '\\';

    public TokenTree Parse(string text)
    {
        var root = new TokenTree();
        var lines = text.Split("\r\n");
        var newLinesCount = 0;

        foreach (var line in lines)
            root.AppendChild(ParseLine(line, newLinesCount++ < lines.Length - 1));
        
        return root;
    }

    private static TokenTree ParseLine(string line, bool appendNewLine)
    {
        var stringBuilder = new StringBuilder();
        var stack = new Stack<TokenTree>();
        var root = new TokenTree();
        var currentNode = root;
        for (var i = 0; i < line.Length; i++)
        {
            var currentChar = line[i];
            
            
            if (currentChar == EscapeChar)
            {
                stringBuilder.Append(GetCharToAppendOnEscape(line, ref i));
                continue;
            }
            if (!MdRules.IsSpecialChar(currentChar))
            {
                stringBuilder.Append(currentChar);
                continue;
            }
            
            var tag = MdRules.ParseTag(stack, line, i, out var nextIndex);

            if (IsClosing(line, tag, stack, currentNode, i))
            {
                FlushTextToChildNode(stringBuilder, currentNode);
                if (currentNode.Tag == tag)
                    currentNode.Close();
                else
                    CloseTag(tag, stack);
                currentNode = stack.Pop();
            }
            else if (Intersects(tag, stack, root))
            {
                stringBuilder.Append(line.AsSpan(i, nextIndex - i + 1));
                var father = stack.Count > 0 ? stack.Peek() : root;
                InvalidateIntersection(tag, father);
            }
            else if (IsOpening(line, tag, stack, i))
            {
                FlushTextToChildNode(stringBuilder, currentNode);
                currentNode = OpenNewNode(currentNode, stack, tag);
            }
            else
            {
                stringBuilder.Append(line.AsSpan(i, nextIndex - i + 1));
            }

            i = nextIndex;
        }

        if (appendNewLine) stringBuilder.Append("\r\n");
        FlushTextToChildNode(stringBuilder, currentNode);
        CloseNestedTags(stack, currentNode);
        
        return root;
    }

    private static void InvalidateIntersection(Tag tag, TokenTree father)
    {
        foreach (var intersection in MdRules.GetPossibleIntersections(tag))
        {
            var intersectionFound = false;
            foreach (var child in father.TaggedChildren())
            {
                if (child.Tag != intersection || !child.Closed) continue;
                if (!child.HasChildrenWithOpenTag(tag)) continue;
                child.Invalidate();
                intersectionFound = true;
                break;
            }

            if (intersectionFound) break;
        }
    }

    private static char GetCharToAppendOnEscape(string text, ref int index)
    {
        if (index + 1 < text.Length && MdRules.IsSpecialChar(text[index + 1]))
            return text[++index];
        
        return text[index];
    }

    private static void CloseNestedTags(Stack<TokenTree> stack, TokenTree currentNode)
    {
        if (currentNode.Tag != null && currentNode.Tag.ClosesOnNewLine)
            currentNode.Close();
        else while (stack.Count > 2)
        {
            var currentLevel = stack.Pop();
            if (currentLevel.Tag == null || !currentLevel.Tag.ClosesOnNewLine) continue;
            currentLevel.Close();
            break;
        }
    }

    private static TokenTree OpenNewNode(TokenTree currentNode, Stack<TokenTree> stack, Tag tag)
    {
        stack.Push(currentNode);
        var newNode = new TokenTree(tag);
        currentNode.AppendChild(newNode);
        return newNode;
    }

    private static void CloseTag(Tag tag, Stack<TokenTree> stack)
    {
        if (stack.Count < 2) return;
        
        var currentLevel = stack.Pop();
        while (currentLevel.Tag != tag)
        {
            currentLevel.Invalidate();
            if (stack.Count == 1)
                return;
            currentLevel = stack.Pop();
        }
        currentLevel.Close();
    }

    private static void FlushTextToChildNode(StringBuilder stringBuilder, TokenTree currentNode)
    {
        if (stringBuilder.Length == 0) return;
        
        var textNode = new TokenTree { Value = stringBuilder.ToString() };
        currentNode.AppendChild(textNode);
        stringBuilder.Clear();
    }
    
    private static bool Intersects(Tag tag, Stack<TokenTree> stack, TokenTree root)
    {
        return MdRules.Intersects(tag, stack, root);
    }

    private static bool IsOpening(string text, Tag tag, Stack<TokenTree> stack, int currentIndex)
    {
        return MdRules.IsValidOpening(tag, stack, text, currentIndex);
    }
    
    private static bool IsClosing(string text, Tag tag, Stack<TokenTree> stack, 
        TokenTree currentNode, int currentIndex)
    {
        return MdRules.IsValidClosing(tag, stack, text, currentIndex) 
               && (currentNode.Tag == tag
                   || stack.Any(node => node.Tag == tag && !node.Closed));
    }
}