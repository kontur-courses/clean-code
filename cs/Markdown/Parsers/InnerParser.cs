using Markdown.Abstractions;
using Markdown.Extensions;
using Markdown.Primitives;

namespace Markdown.Parsers;

public class InnerParser
{
    private readonly Queue<Token> buffer = new();
    private readonly Stack<TokenContext> contexts = new();
    private readonly Dictionary<TokenType, ITokenParser> parsers;
    
    private IEnumerator<Token> enumerator;
    private Token previous;
    
    public Token Current { get; private set; }

    public InnerParser(IEnumerator<Token> enumerator)
    {
        this.enumerator = enumerator;
        
        parsers = new Dictionary<TokenType, ITokenParser>
        {
            { TokenType.Escape, new EscapeParser(this) },
            { TokenType.Italic, new ItalicParser(this) },
            { TokenType.Bold, new BoldParser(this) },
            { TokenType.Header1, new Header1Parser(this) },
            { TokenType.NewLine, new NewLineParser(this) },
            { TokenType.OpenSquareBracket, new LinkParser(this) },
            { TokenType.CloseSquareBracket, new LinkParser(this) },
            { TokenType.Text, new TextParser(this) },

        };
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
    
    public TagNode ParseToken(Token token)
    {
        if (!parsers.TryGetValue(token.Type, out var parser))
        {
            parser = parsers[TokenType.Text];
        }

        return parser.Parse();
    }
    
    public IEnumerable<TagNode> Parse()
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
}