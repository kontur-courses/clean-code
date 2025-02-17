using System.Collections.Immutable;
using System.Text;

namespace Markdown;

public abstract class TokenMdConverter
{
    private readonly Dictionary<TokenType, string> openTagConvert;
    private readonly Dictionary<TokenType, string> closeTagConvert;

    protected TokenMdConverter()
    {
        openTagConvert = new Dictionary<TokenType, string>
        {
            { TokenType.Italic, ItalicOpen() },
            { TokenType.Bold, BoldOpen() },
            { TokenType.Header, HeaderOpen() },
            { TokenType.Marker, MarkerOpen() },
            { TokenType.MarkerRange, MarkerRangeOpen() }
        };

        closeTagConvert = new Dictionary<TokenType, string>()
        {
            { TokenType.Italic, ItalicClose() },
            { TokenType.Bold, BoldClose() },
            { TokenType.Header, HeaderClose() },
            { TokenType.Marker, MarkerClose() },
            { TokenType.MarkerRange, MarkerRangeClose() }
        };
    }

    public string Convert(IImmutableList<Token> tokens)
    {
        var stringBuilder = new StringBuilder();
        var stack = new Stack<Token>();

        foreach (var token in tokens)
        {
            if (token is { IsTag: false }) stringBuilder.Append(token.Value); // сложно написал как-то.

            else if (token is { Type: TokenType.Italic })
            {
                AddPairTag(stack, stringBuilder, token, TokenType.Italic);
            }

            else if (token is { Type: TokenType.Bold })
            {
                AddPairTag(stack, stringBuilder, token, TokenType.Bold);
            }

            else if (token is { Type: TokenType.Header })
            {
                stringBuilder.Append(openTagConvert[TokenType.Header]);
                stack.Push(token);
            }

            else if (token is { Type: TokenType.MarkerRange })
            {
                AddPairTag(stack, stringBuilder, token, TokenType.MarkerRange);
                stringBuilder.Append('\n');
            }

            else if (token is { Type: TokenType.Marker })
            {
                stringBuilder.Append(openTagConvert[TokenType.Marker]);
                stack.Push(token);
            }

            else if (token is { Type: TokenType.NewLine })
            {
                if (stack.TryPeek(out var outToken) && outToken.Type == TokenType.Header)
                {
                    stack.Pop();
                    stringBuilder.Append(closeTagConvert[TokenType.Header]);
                }
                else if (stack.TryPeek(out outToken) && outToken.Type is TokenType.Marker)
                {
                    stack.Pop();
                    stringBuilder.Append(closeTagConvert[TokenType.Marker]);
                }

                stringBuilder.Append('\n');
            }
        }

        if (stack.TryPeek(out var tokenStart) && tokenStart.Type == TokenType.Header)
        {
            stack.Pop();
            stringBuilder.Append(closeTagConvert[TokenType.Header]);
        }

        if (stack.TryPeek(out tokenStart) && tokenStart.Type is TokenType.Marker)
        {
            stack.Pop();
            stringBuilder.Append(closeTagConvert[TokenType.Marker]);
        }

        if (stack.TryPeek(out tokenStart) && tokenStart.Type is TokenType.MarkerRange)
        {
            stringBuilder.Append(closeTagConvert[TokenType.MarkerRange]);
        }


        return stringBuilder.ToString();
    }

    private void AddPairTag(Stack<Token> stack, StringBuilder stringBuilder, Token token, TokenType tokenType)
    {
        if (stack.TryPeek(out var outToken) && outToken.Type == tokenType)
        {
            stack.Pop();
            stringBuilder.Append(closeTagConvert[tokenType]);
        }
        else
        {
            stack.Push(token);
            stringBuilder.Append(openTagConvert[tokenType]);
        }
    }


    protected abstract string ItalicOpen();
    protected abstract string ItalicClose();

    protected abstract string BoldOpen();
    protected abstract string BoldClose();
    protected abstract string HeaderOpen();
    protected abstract string HeaderClose();

    protected abstract string MarkerRangeOpen();
    protected abstract string MarkerRangeClose();

    protected abstract string MarkerOpen();
    protected abstract string MarkerClose();
}