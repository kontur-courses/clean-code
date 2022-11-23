using System.Text;
using Markdown.Tags;
using Markdown.Token;

namespace Markdown.Parse;

public class MarkdownMarkupParser : IMarkupParser
{
    private readonly HashSet<char> specialChars = new() { '_', '#' };
    private readonly HashSet<Tag> tagsThatCloseOnNewLine = new() { MarkdownTags.Heading };
    private const char EscapeChar = '\\';

    public TokenTree Parse(string text)
    {
        var root = new TokenTree();
        var stack = new Stack<TokenTree>();
        var stringBuilder = new StringBuilder();
        var currentNode = root;
        var currentIndex = 0;

        while (currentIndex < text.Length)
        {
            var currentChar = text[currentIndex];

            if (IsNewLine(text, currentIndex, out var newLineSequence))
            {
                currentIndex += newLineSequence.Length - 1;
                stringBuilder.Append(newLineSequence);
                FlushStringBuilder(stringBuilder, currentNode);
                currentNode = CloseNestedTags(stack, currentNode) ?? root;
            }
            else if (currentChar == EscapeChar)
            {
                if (currentIndex + 1 < text.Length && IsSpecialChar(text[currentIndex + 1]))
                {
                    stringBuilder.Append(text[currentIndex + 1]);
                    currentIndex++;
                }
                else
                {
                    stringBuilder.Append(text[currentIndex]);
                }
            }
            else if (IsSpecialChar(currentChar))
            {
                var tag = MdRules.ParseTag(stack, currentNode, text, currentIndex, 
                    out var nextIndex);

                if (IsClosing(text, tag, stack, currentNode, currentIndex))
                {
                    FlushStringBuilder(stringBuilder, currentNode);
                    if (currentNode.IsTaggedWith(tag)) 
                        currentNode.Close();
                    else 
                        CloseTag(tag, stack);
                    currentNode = stack.Pop();
                }
                else if (Intersects(tag, stack, root))
                {
                    var father = stack.Count > 0 ? stack.Peek() : root;
                    foreach (var intersection in MdRules.GetPossibleIntersections(tag))
                    {
                        var intersectionFound = false;
                        foreach (var child in father.TaggedChildren())
                        {
                            if (!child.IsTaggedWith(intersection) || !child.Closed) continue;
                            if (!child.HasChildrenWithOpenTag(tag)) continue;
                            child.Invalidate();
                            stringBuilder.Append(text.AsSpan(currentIndex, nextIndex - currentIndex + 1));
                            intersectionFound = true;
                            break;
                        }
                        if (intersectionFound) break;
                    }
                } 
                else if (IsOpening(text, tag, stack, currentIndex))
                {
                    FlushStringBuilder(stringBuilder, currentNode);
                    currentNode = OpenNewNode(currentNode, stack, tag);
                }
                else
                    stringBuilder.Append(text.AsSpan(currentIndex, nextIndex - currentIndex + 1));
                currentIndex = nextIndex;
            }
            else
            {
                stringBuilder.Append(currentChar);
            }
            
            currentIndex++;
        }
        
        FlushStringBuilder(stringBuilder, currentNode);
        CloseNestedTags(stack, currentNode);
        
        return root;
    }

    private TokenTree? CloseNestedTags(Stack<TokenTree> stack, TokenTree currentNode)
    {
        if (currentNode.Tag != null && tagsThatCloseOnNewLine.Contains(currentNode.Tag))
            currentNode.Close();
        else while (stack.Count > 2)
        {
            var currentLevel = stack.Pop();
            if (currentLevel.Tag == null) continue;
            if (!tagsThatCloseOnNewLine.Contains(currentLevel.Tag)) continue;
            currentLevel.Close();
            break;
        }
        
        return stack.Count > 0 ? stack.Peek() : null;
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
               && (currentNode.IsTaggedWith(tag) 
                   || stack.Any(node => node.IsTaggedWith(tag) && !node.Closed));
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
        while (!currentLevel.IsTaggedWith(tag))
        {
            currentLevel.Invalidate();
            if (stack.Count == 1)
                return;
            currentLevel = stack.Pop();
        }
        currentLevel.Close();
    }

    private static void FlushStringBuilder(StringBuilder stringBuilder, TokenTree currentNode)
    {
        if (stringBuilder.Length == 0) return;
        
        var textNode = new TokenTree { Value = stringBuilder.ToString() };
        currentNode.AppendChild(textNode);
        stringBuilder.Clear();
    }

    private static bool IsNewLine(string text, int currentIndex, out string newLineSequence)
    {
        switch (text[currentIndex])
        {
            case '\n':
                newLineSequence = "\n";
                return true;
            case '\r' when currentIndex + 1 < text.Length && text[currentIndex + 1] == '\n':
                newLineSequence = "\r\n";
                return true;
            default:
                newLineSequence = "";
                return false;
        }
    }

    private bool IsSpecialChar(char c)
    {
        return specialChars.Contains(c);
    }
}