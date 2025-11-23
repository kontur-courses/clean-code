namespace Markdown;

public class TextParser
{
    private int pos;
    private List<Token> tokens = null!;
    private Token Current => pos < tokens.Count ? tokens[pos] : Token.EndOfFile();
    private void Advance() => pos++;

    public string Parse(List<Token> tokensList)
    {
        tokens = tokensList;
        pos = 0;
        var document = new DocumentNode();

        while (Current.Type != TokenType.EndOfFile)
        {
            switch (Current.Type)
            {
                case TokenType.NewLine:
                    continue;
                case TokenType.ListItem:
                {
                    document.Children.Add(ParseList());
                    break;
                }
                default:
                    ParseBlockType(document);
                    break;
            }
        }

        return document.ConvertToHtml();

    }

    private void ParseBlockType(DocumentNode document)
    {
        var isHeading = Current.Type == TokenType.Heading;
        var level = 0;
                
        if (isHeading)
            level = GetHeadingLevel();
                
        var contentNodes = ParseInlineElements();

        Node blockNode = isHeading 
            ? new HeadingNode(level) 
            : new ParagraphNode();
        blockNode.Children.AddRange(contentNodes);
        document.Children.Add(blockNode);

        if (Current.Type == TokenType.NewLine)
            Advance();
    }

    private int GetHeadingLevel()
    {
        var hashes = Current.Value;
        var level = Math.Clamp(hashes.Length, HeadingNode.MinLevel, HeadingNode.MaxLevel);
        Advance();
        return level;
    }

    private ListNode ParseList()
    {
        var listNode = new ListNode();

        while (Current.Type == TokenType.ListItem)
        {
            var listItemNode = ParseListItem();
            listNode.Children.Add(listItemNode);
        }

        return listNode;
    }

    private ListItemNode ParseListItem()
    {
        Advance();

        var listItemNode = new ListItemNode
        {
            Children = ParseInlineElements()
        };

        if (Current.Type == TokenType.NewLine)
            Advance();

        return listItemNode;
    }

    private List<Node> ParseInlineElements()
    {
        var contentNodes = new List<Node>();

        while (HasContent())
        {
            switch (Current.Type)
            {
                case TokenType.BoldStart:
                    contentNodes.Add(ParseStyledNode(TokenType.BoldStart,
                        innerNodes => new BoldNode { Children = innerNodes }));
                    break;
                case TokenType.ItalicStart:
                    contentNodes.Add(ParseStyledNode(TokenType.ItalicStart,
                        innerNodes => new ItalicNode { Children = innerNodes }));
                    break;
                case TokenType.Text:
                    contentNodes.Add(new TextNode(Current.Value));
                    Advance();
                    break;
                case TokenType.ListItem:
                    return contentNodes;
                default:
                    Advance();
                    break;
            }
        }

        return contentNodes;
    }

    private Node ParseStyledNode(TokenType startType, Func<List<Node>, Node> factory)
    {
        var endType = MatchEndType(startType);

        Advance();
        var nodes = new List<Node>();

        while (CanContinueInlineParsing(endType))
        {
            switch (Current.Type)
            {
                case TokenType.Text:
                    nodes.Add(new TextNode(Current.Value));
                    Advance();
                    break;
                case TokenType.ItalicStart:
                    nodes.Add(ParseStyledNode(TokenType.ItalicStart, c => new ItalicNode { Children = c }));
                    break;
                case TokenType.BoldStart:
                    nodes.Add(ParseStyledNode(TokenType.BoldStart, c => new BoldNode { Children = c }));
                    break;
                default:
                    Advance();
                    break;
            }
        }

        if (Encounter(endType))
            Advance();

        return factory(nodes);
    }

    private bool Encounter(TokenType endType)
    {
        return Current.Type == endType;
    }

    private bool CanContinueInlineParsing(TokenType endType)
    {
        return Current.Type != TokenType.EndOfFile && Current.Type != endType &&
               Current.Type != TokenType.NewLine && Current.Type != TokenType.ListItem;
    }

    private static TokenType MatchEndType(TokenType startType)
    {
        var endType = startType switch
        {
            TokenType.ItalicStart => TokenType.ItalicEnd,
            TokenType.BoldStart => TokenType.BoldEnd,
            _ => startType
        };
        return endType;
    }

    private bool HasContent()
    {
        return Current.Type != TokenType.NewLine &&
               Current.Type != TokenType.EndOfFile &&
               Current.Type != TokenType.ListItem;
    }
}