using System.Text;
using Markdown.Parsing.Nodes;
using Markdown.Tokenizing.Tokens;

namespace Markdown.Parsing.Rules;

public class LinkRule : IParsingRule
{
    public bool CanHandle(Token token) =>
        token.Type == TokenType.LeftSquareBracket ||
        token.Type == TokenType.RightSquareBracket ||
        token.Type == TokenType.LeftBracket ||
        token.Type == TokenType.RightBracket;

    public void Handle(ParsingContext context)
    {
        var token = context.CurrentToken;

        if (token.Type == TokenType.LeftSquareBracket)
        {
            var link = TryParseLink(context);
            if (link != null)
            {
                context.Nodes.Add(link);
            }
            else
            {
                context.Nodes.Add(new TextNode { Content = "[" });
                context.MoveForward();
            }
        }
        else
        {
            var text = token.Type switch
            {
                TokenType.RightSquareBracket => "]",
                TokenType.LeftBracket => "(",
                TokenType.RightBracket => ")",
                _ => ""
            };

            context.Nodes.Add(new TextNode { Content = text });
            context.MoveForward();
        }
    }

    private LinkNode? TryParseLink(ParsingContext context)
    {
        var startIndex = context.Index;

        var rightSquareBracketIndex = FindToken(context, startIndex + 1, TokenType.RightSquareBracket);
        if (rightSquareBracketIndex == -1)
            return null;

        if (rightSquareBracketIndex == startIndex + 1)
            return null;

        if (rightSquareBracketIndex + 1 >= context.Tokens.Count ||
            context.Tokens[rightSquareBracketIndex + 1].Type != TokenType.LeftBracket)
            return null;

        var leftBracketIndex = rightSquareBracketIndex + 1;

        var rightBracketIndex = FindToken(context, leftBracketIndex + 1, TokenType.RightBracket);
        if (rightBracketIndex == -1)
            return null;

        if (rightBracketIndex == leftBracketIndex + 1)
            return null;

        var labelNodes = ParseLabel(context, startIndex + 1, rightSquareBracketIndex);

        var url = ParseUrl(context, leftBracketIndex + 1, rightBracketIndex);

        context.Index = rightBracketIndex + 1;

        return new LinkNode
        {
            Url = url,
            Children = labelNodes
        };
    }

    private int FindToken(ParsingContext context, int start, TokenType type)
    {
        var depth = 0;

        for (var i = start; i < context.Tokens.Count; i++)
        {
            var token = context.Tokens[i];

            switch (type)
            {
                case TokenType.RightSquareBracket when token.Type == TokenType.LeftSquareBracket:
                    depth++;
                    break;
                case TokenType.RightSquareBracket:
                {
                    if (token.Type == TokenType.RightSquareBracket)
                    {
                        if (depth == 0)
                            return i;
                        depth--;
                    }

                    break;
                }
                case TokenType.RightBracket when token.Type == TokenType.LeftBracket:
                    depth++;
                    break;
                case TokenType.RightBracket:
                {
                    if (token.Type == TokenType.RightBracket)
                    {
                        if (depth == 0)
                            return i;
                        depth--;
                    }

                    break;
                }
                default:
                {
                    if (token.Type == type)
                    {
                        return i;
                    }

                    break;
                }
            }
        }

        return -1;
    }

    private List<MarkdownNode> ParseLabel(ParsingContext context, int from, int to)
    {
        var tempContext = new ParsingContext
        {
            Tokens = context.Tokens,
            Index = from,
            Nodes = new List<MarkdownNode>(),
            OpenedUnderscores = new Stack<UnderscoreInfo>()
        };

        var underscoreRule = new UnderscoreRule();
        var escapeRule = new EscapeRule();

        while (tempContext.Index < to)
        {
            var token = tempContext.CurrentToken;

            switch (token.Type)
            {
                case TokenType.Text:
                    tempContext.Nodes.Add(new TextNode { Content = token.Value });
                    tempContext.MoveForward();
                    break;
                case TokenType.WhiteSpace:
                    tempContext.Nodes.Add(new TextNode { Content = " " });
                    tempContext.MoveForward();
                    break;
                case TokenType.Underscore:
                    underscoreRule.Handle(tempContext);
                    break;
                case TokenType.EscapeCharacter:
                    escapeRule.Handle(tempContext);
                    break;
                default:
                {
                    var text = token.Type switch
                    {
                        TokenType.Hashtag => "#",
                        TokenType.LeftSquareBracket => "[",
                        TokenType.RightSquareBracket => "]",
                        TokenType.LeftBracket => "(",
                        TokenType.RightBracket => ")",
                        _ => ""
                    };

                    if (!string.IsNullOrEmpty(text))
                        tempContext.Nodes.Add(new TextNode { Content = text });

                    tempContext.MoveForward();
                    break;
                }
            }
        }

        while (tempContext.OpenedUnderscores.Count > 0)
        {
            var underscore = tempContext.OpenedUnderscores.Pop();
            tempContext.Nodes.Insert(underscore.NodeIndex,
                new TextNode { Content = new string('_', underscore.Count) });
        }

        return tempContext.Nodes;
    }

    private string ParseUrl(ParsingContext context, int from, int to)
    {
        var urlBuilder = new StringBuilder();

        for (var i = from; i < to; i++)
        {
            var token = context.Tokens[i];

            switch (token.Type)
            {
                case TokenType.EscapeCharacter:
                {
                    i++;
                    if (i < to)
                    {
                        var nextToken = context.Tokens[i];
                        if (nextToken.Type == TokenType.RightBracket)
                            urlBuilder.Append(')');
                        else if (nextToken.Type == TokenType.EscapeCharacter)
                            urlBuilder.Append('\\');
                        else
                        {
                            urlBuilder.Append('\\');
                            urlBuilder.Append(nextToken.Value);
                        }
                    }
                    else
                    {
                        urlBuilder.Append('\\');
                    }

                    break;
                }
                case TokenType.Text:
                    urlBuilder.Append(token.Value);
                    break;
                case TokenType.WhiteSpace:
                    urlBuilder.Append(' ');
                    break;
                default:
                {
                    var text = token.Type switch
                    {
                        TokenType.Underscore => "_",
                        TokenType.Hashtag => "#",
                        _ => ""
                    };
                    urlBuilder.Append(text);
                    break;
                }
            }
        }

        return urlBuilder.ToString();
    }
}