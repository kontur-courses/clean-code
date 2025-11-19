using Markdown.Parsers.Interfaces;

namespace Markdown.Parsers;

public class UnderscoresParser : ITokenParser, ICompletableParse
{
    private readonly ParserContext context;
    private readonly Stack<Token> markdownUnderscores = new();

    public UnderscoresParser(ParserContext context)
    {
        this.context = context;
    }

    public void Parse()
    {
        if (context.Current.Type == TokenType.Space)
        {
            ParseSpace();
        }

        if (context.Current.Type is TokenType.Underscore or TokenType.DoubleUnderscore)
        {
            ParseUnderscore();
        }
        
        if (context.Current.Type is TokenType.WordUnderscore or TokenType.WordDoubleUnderscore)
        {
            CanParseWordUnderscores();
        }
    }
    public void Finish()
    {
        if (markdownUnderscores.Count > 0)
        {
            ConvertStackToText();
        }
    }
    
    private void ParseUnderscore()
    {
        HandleUnderscore(CanOpen(), CanClose(), IsValidOpenUnderscores, IsValidCloseUnderscores);
    }

    private void CanParseWordUnderscores()
    {
        if (!IsValidWordUnderscores())
        {
            context.Current.Type = TokenType.Text;
        }

        HandleUnderscore(CanOpenWordUnderscore(), CanCloseWordUnderscore(), _ => true, _ => true);
    }
    
    private void HandleUnderscore(
        bool canOpen,
        bool canClose,
        Func<Token, bool> isValidOpen,
        Func<Token, bool> isValidClose)
    {
        if (canOpen && canClose)
        {
            if (markdownUnderscores.Count > 0)
            {
                var peeked = markdownUnderscores.Peek();
                if (!IsIntersecting(peeked))
                    markdownUnderscores.Pop();
            }
            else
            {
                markdownUnderscores.Push(context.Current);
            }
        }
        else if (canOpen)
        {
            HandleOpen(isValidOpen);
        }
        else if (canClose && markdownUnderscores.Count > 0)
        {
            HandleClose(isValidClose);
        }
        else
        {
            context.Current.Type = TokenType.Text;
        }
    }

    private void HandleOpen(Func<Token, bool> isValidOpen)
    {
        if (markdownUnderscores.Count > 0)
        {
            var peeked = markdownUnderscores.Peek();
            if (!isValidOpen(peeked))
            {
                context.Current.Type = TokenType.Text;
            }
            else
            {
                markdownUnderscores.Push(context.Current);
            }
        }
        else
        {
            markdownUnderscores.Push(context.Current);
        }
    }

    private void HandleClose(Func<Token, bool> isValidClose)
    {
        var peeked = markdownUnderscores.Peek();
        if (!isValidClose(peeked) && markdownUnderscores.Count == 1)
        {
            context.Current.Type = TokenType.Text;
        }
        else if (IsIntersecting(peeked))
        {
            markdownUnderscores.Push(context.Current);
            ConvertStackToText();
        }
        else
        {
            markdownUnderscores.Pop();
        }
    }
    private void ParseSpace()
    {
        if (markdownUnderscores.Count <= 0)
        {
            return;
        }
        
        var peeked = markdownUnderscores.Peek();
        if (peeked.Type is TokenType.WordUnderscore or TokenType.WordDoubleUnderscore)
        {
            peeked.Type = TokenType.Text;
            markdownUnderscores.Pop();
        }
    }

    private bool IsValidWordUnderscores()
    {
        if (char.IsDigit(context.Next!.Value[0]) && char.IsDigit(context.Prev!.Value[^1]))
            return false;

        if (char.IsDigit(context.Next!.Value[0]) && char.IsLetter(context.Prev!.Value[^1]))
            return false;
     
        if (char.IsDigit(context.Next!.Value[^1]) && char.IsLetter(context.Prev!.Value[0]))
            return false;

        return true;
    }

    private bool IsValidOpenUnderscores(Token peeked)
    {
        var curr = context.Current.Type;
        var peekedType = peeked.Type;
        
        if (curr == peekedType)
            return false;
        
        return curr != TokenType.DoubleUnderscore || peekedType != TokenType.Underscore;
    }
    
    private bool IsValidCloseUnderscores(Token peeked)
    {
        var currentType = context.Current.Type;
        var peekedType = peeked.Type;
        
        return currentType != TokenType.DoubleUnderscore || peekedType != TokenType.Underscore;
    }

    private bool IsIntersecting(Token peeked)
    {
        switch (context.Current.Type)
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

    private void ConvertStackToText()
    {
        while (markdownUnderscores.Count > 0)
        {
            var token = markdownUnderscores.Pop();
            token.Type = TokenType.Text;
        }
    }

    private bool CanOpen()
    {
        var nextToken = context.Next;
        
        return nextToken != null && !nextToken.Value.StartsWith(" ");
    }

    private bool CanClose()
    {
        if (context.Prev == null || context.Prev.Value.EndsWith(" ") || context.Prev.Type is TokenType.Underscore or TokenType.DoubleUnderscore)
            return false;

        return context.Next == null || context.Next.Value.StartsWith(" ") || context.Next.Type is TokenType.Underscore or TokenType.DoubleUnderscore;
    }

    private bool CanOpenWordUnderscore()
    {
        var current = context.Current;
        if (markdownUnderscores.Count <= 0)
            return true;
        
        var peeked = markdownUnderscores.Peek();
        return current.Type switch
        {
            TokenType.WordUnderscore => peeked.Type is not (TokenType.Underscore or TokenType.WordUnderscore),
            TokenType.WordDoubleUnderscore => peeked.Type switch
            {
                TokenType.DoubleUnderscore or TokenType.WordDoubleUnderscore or TokenType.WordUnderscore
                    or TokenType.Underscore => false,
                _ => true
            },
            _ => true
        };
    }

    private bool CanCloseWordUnderscore()
    {
        var current = context.Current;
        if (markdownUnderscores.Count <= 0)
            return false;
        
        var peeked = markdownUnderscores.Peek();

        if (current.Type is TokenType.WordUnderscore)
        {
            return peeked.Type is TokenType.WordUnderscore or TokenType.Underscore;
        }
        
        return peeked.Type is TokenType.DoubleUnderscore or TokenType.WordDoubleUnderscore;
        
    }
}