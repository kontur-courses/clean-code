using Markdown.Nodes.Interfaces;
using Markdown.Nodes.Internal;
using Markdown.Nodes.Leaf;
using Markdown.Parsers.Interfaces;
using Markdown.Tokenizer;

namespace Markdown.Parsers;

public class MarkdownParser
{
    
    public readonly Stack<MarkdownNode> ParentStack = new();
    public MarkdownNode CurrentParent;
    public Token CurrentToken => tokens[position];
    public Token? NextToken => position + 1 == tokens.Count ? null : tokens[position + 1];
    public Token? PrevToken => position - 1 == -1 ? null : tokens[position - 1];
    
    private readonly List<IParser> parsers;
    private readonly List<Token> tokens;
    private int position;
    public MarkdownParser(List<Token> tokens)
    {
        this.tokens = tokens;
        parsers = new List<IParser>
        {
            new HeaderParser(this),
            new ImageParser(this),
            new UnderscoresParser(this),
            new EscapeParser(this)
        };
        var root = new MarkdownDocumentNode("");
        CurrentParent = root;
    }

    public MarkdownNode ParseTokens(
        HashSet<TokenType>? rollbackStopSymbols = null,
        HashSet<TokenType>? expectedStopSymbols = null)
    {
        while (CurrentToken.Type != TokenType.Eof)
        {
            var parseStatus = TryParseNode(out var node);

            if (parseStatus.Type is not ParseResultType.Success)
                if (parseStatus.Type is not ParseResultType.WasRollback)
                {
                    if (expectedStopSymbols != null && expectedStopSymbols.Contains(CurrentToken.Type))
                    {
                        var currNode = CurrentParent;
                        if (ParentStack.Count > 0)
                            CurrentParent = ParentStack.Pop();
                        return currNode;
                    }

                    if (parseStatus.Type == ParseResultType.NeedRollback
                        || (rollbackStopSymbols != null && rollbackStopSymbols.Contains(CurrentToken.Type)))
                    {
                        var children = Rollback(CurrentParent);
                        var currNode = CurrentParent;
                        CurrentParent = ParentStack.Pop();
                        CurrentParent.AddChildren(children);

                        return CurrentToken.Type != TokenType.Eof ? currNode : CurrentParent;
                    }
                }

            CurrentParent.AddChild(node);
            if (CurrentToken.Type != TokenType.Eof)
                MoveNext();
        }

        return CurrentParent;
    }

    private ParseStatus TryParseNode(out MarkdownNode node)
    {
        foreach (var parser in parsers)
        {
            var status = parser.TryParse(out var parsedNode);
            if (status.Type is ParseResultType.Fail)
                continue;
            node = parsedNode;
            return status;
        }

        node = new TextNode(CurrentToken.Value);
        
        return ParseStatus.Fail();
    }

    public static List<MarkdownNode> Rollback(MarkdownNode node)
    {
        var result = new List<MarkdownNode>();
        RollBackRecursive(node, result);
        
        return result;
    }

    private static void RollBackRecursive(MarkdownNode node, List<MarkdownNode> result)
    {
        result.Add(new TextNode(node.Value));
        foreach (var child in node.Children) 
            RollBackRecursive(child, result);
    }

    public void MoveNext()
    {
        position++;
    }
}