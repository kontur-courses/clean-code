using Markdown.Nodes.Interfaces;
using Markdown.Nodes.Internal;
using Markdown.Nodes.Leaf;
using Markdown.Parsers.Interfaces;
using Markdown.Tokenizer;

namespace Markdown.Parsers;

public class UnderscoresParser : IParser
{
    private readonly HashSet<TokenType> boldExpectedStopSymbols =
    [
        TokenType.DoubleUnderscore,
        TokenType.WordDoubleUnderscore
    ];

    private readonly HashSet<TokenType> boldRollbackSymbols = [TokenType.NewLine, TokenType.Eof];
    private readonly HashSet<TokenType> boldWordRollbackSymbols = [TokenType.Space];

    private readonly HashSet<TokenType> italicExpectedStopSymbols =
    [
        TokenType.Underscore,
        TokenType.WordUnderscore
    ];

    private readonly HashSet<TokenType> italicRollbackSymbols =
    [
        TokenType.DoubleUnderscore,
        TokenType.WordDoubleUnderscore,
        TokenType.NewLine,
        TokenType.Eof
    ];

    private readonly HashSet<TokenType> italicWordRollbackSymbols =
    [
        TokenType.DoubleUnderscore,
        TokenType.WordDoubleUnderscore,
        TokenType.Space
    ];

    private readonly MarkdownParser parser;
    private readonly Stack<Token> underscores = new();

    public UnderscoresParser(MarkdownParser parser)
    {
        this.parser = parser;
    }

    public ParseStatus TryParse(out MarkdownNode node)
    {
        node = new TextNode(parser.CurrentToken.Value);
        var status = ParseStatus.Fail();
        if (parser.CurrentToken.Type is not
            (TokenType.Underscore or TokenType.DoubleUnderscore
            or TokenType.WordUnderscore or TokenType.WordDoubleUnderscore))
            return status;

        if (parser.CurrentToken.Type is TokenType.Underscore or TokenType.DoubleUnderscore)
        {
            status = TryReadUnderscores(out var parsedUnderscoresNode);
            node = parsedUnderscoresNode;
            return status;
        }

        status = TryReadWordUnderscores(out var parsedWordUnderscoresNode);
        node = parsedWordUnderscoresNode;
        return status;
    }

    private ParseStatus TryReadUnderscores(out MarkdownNode node)
    {
        node = new TextNode(parser.CurrentToken.Value);
        
        return TryReadUnderscoreBase(
            CanOpen,
            CanClose,
            t => t == TokenType.Underscore
                ? italicExpectedStopSymbols
                : boldExpectedStopSymbols,
            t => t is TokenType.Underscore or TokenType.WordUnderscore
                ? italicRollbackSymbols
                : boldRollbackSymbols,
            () =>
            {
                var rollback = new HashSet<TokenType>
                {
                    TokenType.Underscore,
                    TokenType.WordUnderscore
                };
                rollback.UnionWith(italicRollbackSymbols);
                return rollback;
            },
            supportNeedRollbackStatus: true,
            out node);
    }

    private ParseStatus TryReadWordUnderscores(out MarkdownNode node)
    {
        node = new TextNode(parser.CurrentToken.Value);
        if (parser.CurrentToken.Type is not (TokenType.WordUnderscore or TokenType.WordDoubleUnderscore))
            return ParseStatus.Fail();
        
        return TryReadUnderscoreBase(
            canOpenFunc: () => !CanCloseWordUnderscore(),
            canCloseFunc: CanCloseWordUnderscore,
            getExpectedStopSymbols: t =>
                t == TokenType.WordUnderscore
                    ? italicExpectedStopSymbols
                    : boldExpectedStopSymbols,
            getRollbackSymbols: t => t is TokenType.Underscore or TokenType.WordUnderscore
                ? italicWordRollbackSymbols
                : boldWordRollbackSymbols,
            getFallbackRollback: () =>
            {
                var rollback = new HashSet<TokenType>
                {
                    TokenType.Underscore,
                    TokenType.WordUnderscore
                };
                rollback.UnionWith(italicWordRollbackSymbols);
                return rollback;
            },
            supportNeedRollbackStatus: false,
            out node);
    }
    
    private ParseStatus TryReadUnderscoreBase(
    Func<bool> canOpenFunc,
    Func<bool> canCloseFunc,
    Func<TokenType, HashSet<TokenType>> getExpectedStopSymbols,
    Func<TokenType, HashSet<TokenType>> getRollbackSymbols,
    Func<HashSet<TokenType>> getFallbackRollback,
    bool supportNeedRollbackStatus,
    out MarkdownNode node)
    {
        node = new TextNode(parser.CurrentToken.Value);
        var underscoreType = parser.CurrentToken.Type;

        if (parser.NextToken?.Type == underscoreType)
            return ParseStatus.Fail();

        var canOpen = canOpenFunc();
        var canClose = canCloseFunc();

        var handled = TryHandleUnderscore(
            canOpen,
            canClose,
            IsValidOpenUnderscores,
            IsValidCloseUnderscores,
            out var handledNode,
            underscoreType);

        if (handled && canOpen)
        {
            parser.ParentStack.Push(parser.CurrentParent);
            parser.CurrentParent = handledNode;
            node = handledNode;

            var expectedStopSymbols = getExpectedStopSymbols(underscoreType);
            var rollbackSymbols = getRollbackSymbols(underscoreType);

            parser.MoveNext();
            parser.ParseTokens(rollbackSymbols, expectedStopSymbols);

            if (supportNeedRollbackStatus &&
                rollbackSymbols.Contains(parser.CurrentToken.Type))
            {
                node = new TextNode($"{parser.CurrentToken.Value}");

                if (parser.CurrentToken.Type is TokenType.Eof or TokenType.NewLine)
                {
                    underscores.Clear();
                    if (parser.CurrentToken.Type == TokenType.Eof)
                        return ParseStatus.NeedRollback();
                }

                return ParseStatus.WasRollback();
            }

            return expectedStopSymbols.Contains(parser.CurrentToken.Type)
                ? ParseStatus.Ok()
                : ParseStatus.Fail();
        }

        if (!handled && canOpen)
        {
            var rollback = getFallbackRollback();
            parser.ParentStack.Push(parser.CurrentParent);
            parser.CurrentParent = GetNodeFromUnderscoreType(underscoreType);
            parser.MoveNext();
            parser.ParseTokens(rollback);
            node = new TextNode($"{parser.CurrentToken.Value}");
            
            return ParseStatus.Ok();
        }

        if (supportNeedRollbackStatus && canClose)
            return ParseStatus.NeedRollback();

        return ParseStatus.Fail();
    }
    
    private bool TryHandleUnderscore(
        bool canOpen,
        bool canClose,
        Func<Token, bool> isValidOpen,
        Func<Token, bool> isValidClose,
        out MarkdownNode node,
        TokenType type)
    {
        if (canOpen && canClose)
        {
            if (underscores.Count > 0)
            {
                var peeked = underscores.Peek();
                if (!IsIntersecting(peeked))
                    underscores.Pop();
            }
            else
            {
                underscores.Push(parser.CurrentToken);
            }
        }
        else if (canOpen)
        {
            if (TryHandleOpen(isValidOpen, type, out var node1))
            {
                node = node1;
                return true;
            }
        }
        else if (canClose && underscores.Count > 0)
        {
            if (TryHandleClose(isValidClose, out var node2))
            {
                node = node2;
                return true;
            }
        }

        node = new TextNode($"{parser.CurrentToken.Value}");
        return false;
    }

    private bool TryHandleOpen(Func<Token, bool> isValidOpen, TokenType type, out MarkdownNode node)
    {
        var value = GetValueFromUnderscoreType(type);
        if (underscores.Count > 0)
        {
            var peeked = underscores.Peek();
            if (!isValidOpen(peeked))
            {
                underscores.Push(parser.CurrentToken);
                node = new TextNode(value);
                return false;
            }

            underscores.Push(parser.CurrentToken);
        }
        else
        {
            underscores.Push(parser.CurrentToken);
        }

        node = GetNodeFromUnderscoreType(type);
        return true;
    }

    private bool TryHandleClose(Func<Token, bool> isValidClose, out MarkdownNode node)
    {
        var peeked = underscores.Peek();
        node = new TextNode("");
        if (!isValidClose(peeked) && underscores.Count == 1)
        {
            underscores.Pop();
            return false;
        }

        if (IsIntersecting(peeked))
        {
            underscores.Pop();
            return false;
        }

        underscores.Pop();

        return true;
    }

    private bool IsValidOpenUnderscores(Token peeked)
    {
        var curr = parser.CurrentToken.Type;
        var peekedType = peeked.Type;

        if (curr == peekedType)
            return false;

        return curr switch
        {
            TokenType.Underscore => peekedType is TokenType.DoubleUnderscore,
            TokenType.WordUnderscore => peekedType is TokenType.DoubleUnderscore or TokenType.WordDoubleUnderscore,
            TokenType.DoubleUnderscore => peekedType is not TokenType.Underscore,
            TokenType.WordDoubleUnderscore => peekedType != TokenType.Underscore &&
                                              peekedType != TokenType.WordUnderscore,
            _ => true
        };
    }

    private bool IsValidCloseUnderscores(Token peeked)
    {
        var currentType = parser.CurrentToken.Type;
        var peekedType = peeked.Type;

        if (currentType == TokenType.Underscore) return peekedType is TokenType.Underscore or TokenType.WordUnderscore;

        return peekedType is TokenType.DoubleUnderscore or TokenType.WordDoubleUnderscore;
    }

    private bool CanOpen()
    {
        return parser.NextToken is { Type: TokenType.Text } &&
               !parser.NextToken.Value.StartsWith(" ");
    }

    private bool CanClose()
    {
        if (underscores.Count == 0
            || parser.PrevToken == null
            || parser.PrevToken.Value.EndsWith(" ")
            || parser.PrevToken.Type is TokenType.Underscore or TokenType.DoubleUnderscore)
            return false;

        return parser.NextToken != null;
    }
    private bool CanCloseWordUnderscore()
    {
        if (underscores.Count <= 0)
            return false;

        var peeked = underscores.Peek();

        if (parser.CurrentToken.Type is TokenType.WordUnderscore)
            return peeked.Type is TokenType.WordUnderscore or TokenType.Underscore;

        return peeked.Type is TokenType.DoubleUnderscore or TokenType.WordDoubleUnderscore;
    }

    private bool IsIntersecting(Token peeked)
    {
        switch (parser.CurrentToken.Type)
        {
            case TokenType.Underscore:
            case TokenType.WordUnderscore:
            {
                if (peeked.Type is TokenType.DoubleUnderscore or TokenType.WordDoubleUnderscore)
                    return true;
                break;
            }
            case TokenType.DoubleUnderscore:
            case TokenType.WordDoubleUnderscore:
            {
                if (peeked.Type is TokenType.Underscore or TokenType.WordUnderscore)
                    return true;
                break;
            }
        }

        return false;
    }

    private string GetValueFromUnderscoreType(TokenType type)
    {
        return type switch
        {
            TokenType.Underscore or TokenType.WordUnderscore => "_",
            TokenType.DoubleUnderscore or TokenType.WordDoubleUnderscore => "__",
            _ => throw new ArgumentException("Token type must be Underscore or WordUnderscore or DoubleUnderscore or " +
                                             "WordDoubleUnderscore")
        };
    }

    private MarkdownNode GetNodeFromUnderscoreType(TokenType type)
    {
        var value = GetValueFromUnderscoreType(type);

        return type switch
        {
            TokenType.Underscore or TokenType.WordUnderscore => new ItalicNode(value),
            TokenType.DoubleUnderscore or TokenType.WordDoubleUnderscore => new BoldNode(value),
            _ => throw new ArgumentException("Token type must be Underscore or WordUnderscore or DoubleUnderscore or " +
                                             "WordDoubleUnderscore")
        };
    }
}