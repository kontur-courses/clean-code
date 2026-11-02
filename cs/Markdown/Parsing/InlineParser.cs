using Markdown.Nodes;
using System.Text;

namespace Markdown.Parsing;

public class InlineParser
{
    public List<INode> Parse(string text)
    {
        var tokens = Tokenize(text);
        var validPairs = FindValidPairs(tokens, text);
        var rootNodes = new List<INode>();

        var stack = new Stack<(TokenType Type, List<INode> ParentList)>();
        var currentList = rootNodes;

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];

            if (token.Type == TokenType.Text)
            {
                currentList.Add(new TextNode(token.Value));
                continue;
            }

            if (validPairs.Contains((i, true)))
            {
                stack.Push((token.Type, currentList));
                currentList = new();
            }
            else if (validPairs.Contains((i, false)))
            {
                var (type, parentList) = stack.Pop();
                var htmlTag = type.ToHtmlTag();

                var container = new ContainerNode(htmlTag, currentList);
                currentList = parentList;
                currentList.Add(container);
            }
            else
            {
                currentList.Add(new TextNode(token.Value));
            }
        }

        return rootNodes;
    }

    private List<Token> Tokenize(string text)
    {
        var tokens = new List<Token>();
        var currentText = new StringBuilder();

        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];

            if (c == '\\' && i + 1 < text.Length && TryEscape(text[i + 1]) is char escapedChar)
            {
                currentText.Append(escapedChar);
                i += 1;
                continue;
            }

            if (c == '_')
            {
                FlushText(tokens, currentText, i);

                var skip = ProcessUnderscores(text, i, tokens, currentText);
                i += skip;
            }
            else
            {
                currentText.Append(c);
            }
        }

        FlushText(tokens, currentText, text.Length);
        return tokens;
    }

    private void FlushText(List<Token> tokens, StringBuilder currentText, int currentIndex)
    {
        if (currentText.Length > 0)
        {
            tokens.Add(new Token(TokenType.Text, currentText.ToString(), currentIndex - currentText.Length));
            currentText.Clear();
        }
    }

    private int ProcessUnderscores(string text, int index, List<Token> tokens, StringBuilder currentText)
    {
        var isDouble = (index + 1 < text.Length) && (text[index + 1] == '_');
        var isTriple = isDouble && (index + 2 < text.Length) && (text[index + 2] == '_');
        var length = isTriple ? 3 : (isDouble ? 2 : 1);

        if (!IsValidMarkerContext(text, index, length))
        {
            currentText.Append(new string('_', length));
            return length - 1;
        }

        if (isTriple)
        {
            var leftIsBoundary = (index == 0) || char.IsWhiteSpace(text[index - 1]);
            var rightIsBoundary = (index + 3 == text.Length) || char.IsWhiteSpace(text[index + 3]);

            if (rightIsBoundary && !leftIsBoundary)
            {
                tokens.Add(new Token(TokenType.Italic, "_", index));
                tokens.Add(new Token(TokenType.Bold, "__", index + 1));
            }
            else
            {
                tokens.Add(new Token(TokenType.Bold, "__", index));
                tokens.Add(new Token(TokenType.Italic, "_", index + 2));
            }
            return 2; 
        }

        if (isDouble)
        {
            tokens.Add(new Token(TokenType.Bold, "__", index));
            return 1; 
        }

        tokens.Add(new Token(TokenType.Italic, "_", index));
        return 0; 
    }

    private char? TryEscape(char nextChar)
    {
        return nextChar switch
        {
            '_' => '_',
            '\\' => '\\',
            _ => null
        };
    }

    private bool IsValidMarkerContext(string text, int index, int length)
    {
        var leftIndex = index - 1;
        var rightIndex = index + length;

        char? left = leftIndex >= 0 ? text[leftIndex] : null;
        char? right = rightIndex < text.Length ? text[rightIndex] : null;

        if (left.HasValue && right.HasValue && char.IsDigit(left.Value) && char.IsDigit(right.Value))
            return false;

        var leftIsSpace = left.HasValue && char.IsWhiteSpace(left.Value);
        var rightIsSpace = right.HasValue && char.IsWhiteSpace(right.Value);

        return !(leftIsSpace && rightIsSpace);
    }

    private HashSet<(int Index, bool IsOpening)> FindValidPairs(List<Token> tokens, string text)
    {
        var validPairs = new HashSet<(int, bool)>();
        var stack = new Stack<int>();

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (token.Type == TokenType.Text) continue;

            var canBeOpening = IsNotSpaceAfter(text, token);
            var canBeClosing = IsNotSpaceBefore(text, token);

            if (canBeClosing && TryCloseToken(token, i, tokens, text, stack, validPairs))
            {
                continue;
            }

            if (canBeOpening && CanOpenToken(token, tokens, stack))
            {
                stack.Push(i);
            }
        }

        return validPairs;
    }

    private bool TryCloseToken(Token token, int index, List<Token> tokens, string text, Stack<int> stack, HashSet<(int, bool)> validPairs)
    {
        if (stack.Count == 0) return false;

        var matchingOpenIndex = FindMatchingOpenIndex(token.Type, stack, tokens);
        if (matchingOpenIndex == -1) return false;

        if (stack.Peek() != matchingOpenIndex)
        {
            stack.Clear();
            return true;
        }

        if (HasValidContent(matchingOpenIndex, index, token, tokens, text))
        {
            stack.Pop();
            validPairs.Add((matchingOpenIndex, true));
            validPairs.Add((index, false));
            return true;
        }

        return false;
    }

    private bool HasValidContent(int openIndex, int closeIndex, Token closeToken, List<Token> tokens, string text)
    {
        var hasContent = false;
        var hasSpace = false;

        for (var i = openIndex + 1; i < closeIndex; i++)
        {
            if (tokens[i].Type == TokenType.Text)
            {
                if (tokens[i].Value.Trim().Length > 0) hasContent = true;
                if (tokens[i].Value.Contains(' ')) hasSpace = true;
            }
            else
            {
                hasContent = true;
            }
        }

        if (!hasContent) return false;

        if (closeToken.Type == TokenType.Italic && hasSpace)
        {
            var openPos = tokens[openIndex].Position;
            var closePos = closeToken.Position;

            var openIsBoundary = openPos == 0 || !char.IsLetterOrDigit(text[openPos - 1]);
            var closeIsBoundary = closePos + closeToken.Value.Length >= text.Length ||
                                   !char.IsLetterOrDigit(text[closePos + closeToken.Value.Length]);

            return openIsBoundary && closeIsBoundary;
        }

        return true;
    }

    private bool CanOpenToken(Token token, List<Token> tokens, Stack<int> stack)
    {
        if (token.Type == TokenType.Bold && stack.Any(idx => tokens[idx].Type == TokenType.Italic))
        {
            return false;
        }
        return true;
    }

    private int FindMatchingOpenIndex(TokenType type, Stack<int> stack, List<Token> tokens)
    {
        for (var i = stack.Count - 1; i >= 0; i--)
        {
            if (tokens[stack.ElementAt(i)].Type == type)
            {
                return stack.ElementAt(i);
            }
        }
        return -1;
    }

    private bool IsNotSpaceAfter(string text, Token token)
    {
        var index = token.Position + token.Value.Length;
        return index >= text.Length || !char.IsWhiteSpace(text[index]);
    }

    private bool IsNotSpaceBefore(string text, Token token)
    {
        return token.Position == 0 || !char.IsWhiteSpace(text[token.Position - 1]);
    }
}