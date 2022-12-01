using System.Text;
using Markdown.Abstractions;
using Markdown.Extensions;
using Markdown.Primitives;

namespace Markdown.Parsers;

public class TokenCollectionParser : ITokenCollectionParser
{
    private readonly Queue<Token> buffer = new();
    private readonly Stack<TokenContext> contexts = new();
    
    private IEnumerator<Token> enumerator;
    private Token previous;
    
    public Token Current { get; private set; }

    public IEnumerable<TagNode> Parse(IEnumerable<Token> tokens)
    {
        if (tokens == null)
        {
            throw new ArgumentNullException(nameof(tokens));
        }

        enumerator = tokens.GetEnumerator();

        return CombineTextTagNodes(Parse());
    }

    private IEnumerable<TagNode> Parse()
    {
        while (TryMoveNext(out var current))
        {
            var node = ParseToken(current);
            
            if (contexts.TryPeek(out var context))
            {
                context.AddChild(node);
            }
            else
            {
                yield return node;
            }
        }

        if (TryFlushContextsUntil(out var contextNode, TokenContext.IsHeader1))
            yield return ToNode(contextNode);
    }


    public bool TryFlushContextsUntil(out TokenContext tokenContext, Func<TokenContext, bool> isStopper)
    {
        if (isStopper == null)
        {
            throw new ArgumentNullException(nameof(isStopper));
        }

        var stack = new Stack<string>();
        while (contexts.TryPop(out var context))
        {
            if (isStopper(context) || contexts.Count == 0)
            {
                var node = Tokens.Text(string.Join("", stack)).ToTagNode();
                context.AddChild(node);
                tokenContext = context;
                return true;
            }

            stack.Push(context.ToText());
        }

        tokenContext = null!;
        return false;
    }

    public TagNode ParseToken(Token token)
    {
        ITokenParser parser = token.Type switch
        {
            TokenType.Escape => new EscapeParser(this),
            TokenType.Italic => new ItalicParser(this),
            TokenType.Bold => new BoldParser(this),
            TokenType.Header1 => new Header1Parser(this),
            TokenType.NewLine => new NewLineParser(this),
            TokenType.OpenSquareBracket => new LinkParser(this),
            TokenType.CloseSquareBracket => new LinkParser(this),
            _ => new TextParser(this)
        };
        
        return parser.Parse();
    }
    
    public bool TryPopContext(out TokenContext context)
    {
        return contexts.TryPop(out context);
    }

    public void PushContext(TokenContext tokenContext)
    {
        contexts.Push(tokenContext);
    }

    public bool TryGetPreviousToken(out Token token)
    {
        token = previous;
        return token != null;
    }

    public bool TryGetNextToken(out Token token)
    {
        if (buffer.TryPeek(out token))
        {
            return true;
        }
        if (enumerator.MoveNext())
        {
            buffer.Enqueue(enumerator.Current);
            token = enumerator.Current;
            return true;
        }

        return false;
    }

    public bool TryMoveNext(out Token token)
    {
        var result = MoveNext();
        token = result ? enumerator.Current : null!;
        return result;
    }

    private bool MoveNext()
    {
        if (buffer.TryDequeue(out var token))
        {
            if (buffer.Count > 0)
            {
                previous = Current;
            }
            Current = token;
            return true;
        }

        if (enumerator.MoveNext())
        {
            previous = Current;
            Current = enumerator.Current;
            return true;
        }

        return false;
    }

    public void PushToBuffer(Token token)
    {
        buffer.Enqueue(token);
    }

    public TagNode ToNode(TokenContext context)
    {
        return context.Token.Type == TokenType.Header1 && contexts.Count == 0
            ? new TagNode(Tokens.Header1.ToTag(), context.Children.ToArray())
            : Tags.Text(context.ToText()).ToTagNode();
    }

    public bool AnyContext(Func<TokenContext, bool> predicate)
    {
        return contexts.Any(predicate);
    }

    private IEnumerable<TagNode> CombineTextTagNodes(IEnumerable<TagNode> nodes)
    {
        var sb = new StringBuilder();
        foreach (var node in nodes)
        {
            if (node.Tag.Type == TagType.Text)
            {
                sb.Append(node.Tag.Value);
            }
            else
            {
                if (sb.Length > 0)
                {
                    yield return Tags.Text(sb.ToString()).ToTagNode();
                    sb.Clear();
                }

                yield return new TagNode(node.Tag, CombineTextTagNodes(node.Children).ToArray());
            }
        }

        if (sb.Length > 0)
        {
            yield return Tags.Text(sb.ToString()).ToTagNode();
        }
    }
}