using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Parser
    {

            private List<Token> tokens;
            private int position;

            public MainNode ParseTokensToTree(List<Token> tokenList)
            {
                tokens = tokenList;
                position = 0;

                var root = new MainNode();

                while (position < tokens.Count)
                {
                    var node = ParseToken();
                    if (node != null)
                        root.Children.Add(node);
                }

                return root;
            }
            
            private Node? ParseToken(bool inEmphasisContext = false)
            {
                if (position >= tokens.Count)
                    return null;

                var token = tokens[position];

                switch (token.Type)
                {
                    case TokenType.Text:
                    case TokenType.Space:
                        position++;
                        return new TextNode(token.Value);

                    case TokenType.NextLine:
                        position++;
                        return new NextLineNode();

                    case TokenType.Header:
                        return ParseHeader();

                    case TokenType.ListItem:
                        return ParseListItem();

                    case TokenType.Emphasis:
                        return ParseEmphasisNode(inEmphasisContext);

                    case TokenType.Strong:
                        return inEmphasisContext ? ParseStrongAsText(tokens[position]) : ParseStrongNode(inEmphasisContext);

                    default:
                        position++;
                        return new TextNode(token.Value);
                }
            }

            private Node ParseHeader()
            {
         
                position++;

                if (position >= tokens.Count ||
                    tokens[position].Type != TokenType.Space ||
                    tokens[position].Value != " ")
                {
                    return new TextNode("#");
                }

                position++;
                

                var header = new HeaderNode();
                while (position < tokens.Count && tokens[position].Type != TokenType.NextLine)
                    header.Children.Add(ParseToken());
                

                return header;
            }

            private Node ParseListItem()
            {
                var start = position;

                if (!TrySkipListItemPrefix())
                {
                    return new TextNode("-");
                }

                var listNode = new ListNode();
                ParseFirstListItem(listNode);
                ParseNextListItems(listNode);

                return listNode.Children.Count > 0 ? listNode : CreateTextNodeFallback(start);
            }

            private bool TrySkipListItemPrefix()
            {
                if (position >= tokens.Count || tokens[position].Type != TokenType.ListItem)
                    return false;

                position++;

                if (position >= tokens.Count || tokens[position].Type != TokenType.Space)
                    return false;

                position++;
                return true;
            }

            private void ParseFirstListItem(ListNode listNode)
            {
                var firstItem = ParseListItemContent();
                if (firstItem != null)
                    listNode.Children.Add(firstItem);

            }

            private void ParseNextListItems(ListNode listNode)
            {
                while (position < tokens.Count && tokens[position].Type == TokenType.NextLine)
                {
                    position++;

                    if (!TrySkipListItemPrefix())
                        break;

                    var nextItem = ParseListItemContent();
                    if (nextItem != null)
                        listNode.Children.Add(nextItem);
                    else
                        break;
                }
            }

            private ListItemNode? ParseListItemContent()
            {
                var itemNode = new ListItemNode();

                while (position < tokens.Count && tokens[position].Type != TokenType.NextLine)
                {
                    if (tokens[position].Type == TokenType.ListItem)
                    {
                        break;
                    }
                    itemNode.Children.Add(ParseToken());
                }

                return itemNode.Children.Count > 0 ? itemNode : null;
            }

            
            
            
            private Node ParseEmphasisNode(bool inEmphasisContext = false)
            {
                int startPos = position;
                position++;

                if (!IsValidEmphasisOpening(startPos))
                    return new TextNode("_");

                bool isMidWord = IsMidWordEmphasis(startPos);
                var node = new EmphasisNode();
                bool hasSpaceInside = false;


                int closingIndex = FindNextTokenIndex(startPos + 1, TokenType.Emphasis);
                
                
                if (closingIndex == -1)
                    return CreateTextNodeFallback(startPos);
                
                if (!IsValidEmphasisClosing(closingIndex))
                {

                    return CreateTextNodeFallback(startPos);
                }
                
                
                bool strongOpenedInside = false;
                bool strongClosedInside = false;

                while (position < closingIndex && position < tokens.Count)
                {
                    var t = tokens[position];


                    if (t.Type == TokenType.Space || t.Type == TokenType.NextLine)
                        hasSpaceInside = true;

                    if (t.Type == TokenType.Strong)
                    {

                        int nextStrong = FindNextTokenIndex(position + 1, TokenType.Strong);
                        if (nextStrong != -1 && nextStrong < closingIndex)
                            strongClosedInside = true;
                        else
                            strongOpenedInside = true;

          
                        node.Children.Add(new TextNode("__"));
                        position++;
                        continue;
                    }
                    
                    node.Children.Add(ParseToken(inEmphasisContext: true));
                }


                if (position >= tokens.Count || tokens[position].Type != TokenType.Emphasis)
                    return CreateTextNodeFallback(startPos);


                
                
                bool isIntersection = strongOpenedInside && !strongClosedInside;
                if (isIntersection)
                    return CreateTextNodeFallback(startPos);

  
                if (isMidWord && hasSpaceInside)
                    return CreateTextNodeFallback(startPos);

                if (!IsValidEmphasisClosing(position))
                    
                    return CreateTextNodeFallback(startPos);

                position++;
                return node;
            }
            


            private bool IsValidEmphasisClosing(int closingIndex)
            {
                if (closingIndex == 0) 
                    return false;

                var prev = tokens[closingIndex - 1];

                if (prev.Type == TokenType.Space || prev.Type == TokenType.NextLine)
                    return false;

                if (prev.Type == TokenType.Text && prev.Value.Length > 0 && 
                    char.IsDigit(prev.Value[prev.Value.Length - 1]))
                    return false;

                return true;
            }



            private bool IsValidEmphasisOpening(int startPos)
            {
                if (startPos + 1 >= tokens.Count)
                    return false;

                var nextToken = tokens[startPos + 1];
                return IsValidTokenBoundary(nextToken);
            }

            private bool IsMidWordEmphasis(int startPos)
            {
                if (startPos == 0)
                    return false;

                var previousToken = tokens[startPos - 1];
                return previousToken.Type == TokenType.Text;
            }


            private Node ParseStrongNode(bool inEmphasisContext)
            {
                int startPos = position;


                if (!IsValidStrongOpening())
                    return new TextNode(tokens[position++].Value);

                int closingIndex = FindNextTokenIndex(position + 1, TokenType.Strong);
                if (closingIndex == -1)
                    return CreateTextNodeFallback(startPos);

  
                position++;

                var node = new StrongNode();
                bool hasSpaceInside = false;
                bool emphasisOpenedInside = false;
                bool emphasisClosedInside = false;

                while (position < closingIndex && position < tokens.Count)
                {
                    var t = tokens[position];

                    if (t.Type == TokenType.NextLine)
                        break;
                    
                    if (t.Type == TokenType.Space)
                        hasSpaceInside = true;

                    if (t.Type == TokenType.Emphasis)
                    {
                        int nextEm = FindNextTokenIndex(position + 1, TokenType.Emphasis);
                        if (nextEm != -1 && nextEm < closingIndex)
                            emphasisClosedInside = true;
                        else
                            emphasisOpenedInside = true;

                        var inner = ParseEmphasisNode(inEmphasisContext: true);
                        node.Children.Add(inner);
                        continue;
                    }


                    node.Children.Add(ParseToken(inEmphasisContext: true));
                }

                if (position >= tokens.Count || tokens[position].Type != TokenType.Strong)
                    return CreateTextNodeFallback(startPos);


                bool isIntersection = emphasisOpenedInside && !emphasisClosedInside;
                if (isIntersection)
                    return CreateTextNodeFallback(startPos);
                
                if (tokens[startPos + 1].Type == TokenType.Space)
                    return CreateTextNodeFallback(startPos);
                
                if (position > 0 && tokens[position - 1].Type == TokenType.Space)
                    return CreateTextNodeFallback(startPos);
                
                bool isMidWord = IsMidWordStrong(startPos);
                if (isMidWord && hasSpaceInside)
                    return CreateTextNodeFallback(startPos);

                if (!IsValidStrongClosing())
                    return CreateTextNodeFallback(startPos);

                position++;
                return node;
            }
            
            private bool IsMidWordStrong(int startPos)
            {
                if (startPos == 0)
                    return false;

                var previousToken = tokens[startPos - 1];
                return previousToken.Type == TokenType.Text && previousToken.Value.Length > 0;
            }

            private Node ParseStrongAsText(Token token)
            {
                position++;
                return new TextNode(token.Value);
            }

            private bool IsValidStrongOpening()
            {
                if (position + 1 >= tokens.Count)
                    return false;

                var nextToken = tokens[position + 1];
                if (nextToken.Type == TokenType.Space || nextToken.Type == TokenType.NextLine)
                    return false;
                return IsValidTokenBoundary(nextToken);
            }

            private bool IsValidStrongClosing()
            {
                if (position == 0)
                    return false;

                var previousToken = tokens[position - 1];
                if (previousToken.Type == TokenType.Space || previousToken.Type == TokenType.NextLine)
                    return false;
                return IsValidTokenBoundary(previousToken);
            }

            private bool IsValidTokenBoundary(Token token)
            {
                return token.Type != TokenType.Space &&
                       token.Type != TokenType.NextLine &&
                       !(token.Type == TokenType.Text && token.Value.Length > 0 && char.IsDigit(token.Value[0]));
            }



            private Node CreateTextNodeFallback(int startPos)
            {
                var text = new StringBuilder();
                
                if (position == startPos)
                {
                    text.Append(tokens[position].Value);
                    position++;
                }
                else
                {
                    for (int i = startPos; i < position && i < tokens.Count; i++)
                        text.Append(tokens[i].Value);
                }
    
                return new TextNode(text.ToString());
            }


            private int FindNextTokenIndex(int fromInclusive, TokenType type)
            {
                for (int i = fromInclusive; i < tokens.Count; i++)
                    if (tokens[i].Type == type)
                        return i;
                return -1;
            }
        }
    
}