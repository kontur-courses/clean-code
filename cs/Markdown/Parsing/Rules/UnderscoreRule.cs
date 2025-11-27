using Markdown.Parsing.Nodes;
using Markdown.Tokenizing.Tokens;

namespace Markdown.Parsing.Rules;

public class UnderscoreInfo
{
    public int Count { get; set; }
    public int TokenIndex { get; set; }
    public int NodeIndex { get; set; }
}

public class UnderscoreRule : IParsingRule
{
    public bool CanHandle(Token token) => token.Type == TokenType.Underscore;

    public void Handle(ParsingContext context)
    {
        var underscoreCount = CountConsecutiveUnderscores(context);

        var previousToken = context.PreviousToken;
        var nextToken = context.PeekToken(underscoreCount);

        if (IsBetweenDigits(previousToken, nextToken))
        {
            context.Nodes.Add(new TextNode { Content = new string('_', underscoreCount) });
            context.MoveForward(underscoreCount);
            return;
        }

        var existingUnderscore = FindOpenedUnderscore(context.OpenedUnderscores, underscoreCount);

        if (existingUnderscore != null)
        {
            CloseUnderscore(context, existingUnderscore, underscoreCount);
        }
        else
        {
            OpenUnderscore(context, underscoreCount);
        }

        context.MoveForward(underscoreCount);
    }

    private int CountConsecutiveUnderscores(ParsingContext context)
    {
        var count = 0;
        var offset = 0;

        while (context.Index + offset < context.Tokens.Count &&
               context.Tokens[context.Index + offset].Type == TokenType.Underscore)
        {
            count++;
            offset++;
        }

        return count;
    }

    private UnderscoreInfo? FindOpenedUnderscore(Stack<UnderscoreInfo> openedUnderscores, int count)
    {
        return openedUnderscores.FirstOrDefault(u => u.Count == count);
    }

    private void OpenUnderscore(ParsingContext context, int count)
    {
        if (count >= 2)
        {
            var hasOpenedSingle = context.OpenedUnderscores.Any(u => u.Count == 1);
            if (hasOpenedSingle)
            {
                context.Nodes.Add(new TextNode { Content = new string('_', count) });
                return;
            }
        }

        context.OpenedUnderscores.Push(new UnderscoreInfo
        {
            Count = count,
            TokenIndex = context.Index,
            NodeIndex = context.Nodes.Count
        });
    }

    private void CloseUnderscore(ParsingContext context, UnderscoreInfo openedUnderscore, int count)
    {
        var intersectingUnderscores = context.OpenedUnderscores
            .Where(u => u.NodeIndex > openedUnderscore.NodeIndex && u.Count != count)
            .ToList();

        if (intersectingUnderscores.Count != 0)
        {
            context.Nodes.Add(new TextNode { Content = new string('_', count) });
            
            foreach (var intersecting in intersectingUnderscores.OrderByDescending(u => u.NodeIndex))
            {
                context.Nodes.Insert(intersecting.NodeIndex, 
                    new TextNode { Content = new string('_', intersecting.Count) });
            }
            
            context.Nodes.Insert(openedUnderscore.NodeIndex, 
                new TextNode { Content = new string('_', count) });
            
            RemoveUnderscoreFromStack(context.OpenedUnderscores, openedUnderscore);
            foreach (var intersecting in intersectingUnderscores)
            {
                RemoveUnderscoreFromStack(context.OpenedUnderscores, intersecting);
            }
            return;
        }
        
        if (count >= 2)
        {
            var hasSingleAfter = context.OpenedUnderscores.Any(u =>
                u.Count == 1 &&
                u.NodeIndex > openedUnderscore.NodeIndex);

            if (hasSingleAfter)
            {
                context.Nodes.Add(new TextNode { Content = new string('_', count) });
                return;
            }
        }
        
        if (IsInsideWords(context, openedUnderscore, count))
        {
            context.Nodes.Add(new TextNode { Content = new string('_', count) });
            return;
        }
        
        var startIndex = openedUnderscore.NodeIndex;
        var innerNodesCount = context.Nodes.Count - startIndex;
        var innerNodes = context.Nodes.GetRange(startIndex, innerNodesCount);

        // Удаляем эти узлы из общего списка
        context.Nodes.RemoveRange(startIndex, innerNodesCount);
        
        MarkdownNode formattedNode = count >= 2
            ? new BoldNode { Children = innerNodes }
            : new ItalicNode { Children = innerNodes };

        context.Nodes.Add(formattedNode);

        RemoveUnderscoreFromStack(context.OpenedUnderscores, openedUnderscore);
    }

    private void RemoveUnderscoreFromStack(Stack<UnderscoreInfo> stack, UnderscoreInfo toRemove)
    {
        var tempList = stack.ToList();
        tempList.Remove(toRemove);

        stack.Clear();
        for (var i = tempList.Count - 1; i >= 0; i--)
        {
            stack.Push(tempList[i]);
        }
    }

    private bool IsBetweenDigits(Token? previousToken, Token? nextToken)
    {
        var previousCharIsDigit = previousToken is { Type: TokenType.Text, Value: not null } &&
                                  char.IsDigit(previousToken.Value.Last());
        var nextCharIsDigit =
            nextToken is { Type: TokenType.Text, Value: not null } && char.IsDigit(nextToken.Value.First());
        return previousCharIsDigit && nextCharIsDigit;
    }

    private bool HasWhitespaceBetween(ParsingContext context, int startNodeIndex)
    {
        for (var i = startNodeIndex; i < context.Nodes.Count; i++)
        {
            if (context.Nodes[i] is TextNode textNode &&
                textNode.Content.Any(char.IsWhiteSpace))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsInsideWords(ParsingContext context, UnderscoreInfo openedUnderscore, int count)
    {
        var tokenBeforeOpen = context.Tokens.ElementAtOrDefault(openedUnderscore.TokenIndex - 1);
        var hasTextBeforeOpen = tokenBeforeOpen?.Type == TokenType.Text;
        
        var tokenAfterClose = context.PeekToken(count - 1);
        var hasTextAfterClose = tokenAfterClose?.Type == TokenType.Text;
        
        var hasWhitespaceInside = HasWhitespaceBetween(context, openedUnderscore.NodeIndex);
        
        return hasWhitespaceInside && (hasTextBeforeOpen || hasTextAfterClose);
    }
}