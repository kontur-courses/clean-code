using System.Text;
using Markdown.Enums;
using Markdown.Interfaces;
using Markdown.Tokens;

namespace Markdown;

public class TokenSetter : ITokenSetter<TokenType>
{
    private readonly ITokenBuilder tokenBuilder;

    public TokenSetter(ITokenBuilder tokenBuilder)
    {
        this.tokenBuilder = tokenBuilder;
    }

    public void SetToken(List<Token> tokens, TokenType type, ref int index, string line, StringBuilder builder)
    {
        switch (type)
        {
            case TokenType.Text:
                builder.Append(line[index]);
                if (index == line.Length - 1)
                    ClearStringBuilder(tokens, builder, index);
                break;
            case TokenType.Slash:
                if (line[index + 1] != '\\')
                    builder.Append(line[index + 1]);
                index++;
                if (index == line.Length - 1)
                    ClearStringBuilder(tokens, builder, index);
                break;
            case TokenType.Strong:
                ClearStringBuilder(tokens, builder, index);
                AddTag(tokens, type, index, index + 1);
                index++;
                break;
            default:
                ClearStringBuilder(tokens, builder, index);
                AddTag(tokens, type, index, index);
                break;
        }
    }


    public void AddTag(List<Token> tokens, TokenType type, int start, int end)
    {
        var tag = tokenBuilder.GetTag(start, end, type);
        tokens.Add(tag);
    }

    private void ClearStringBuilder(List<Token> tokens, StringBuilder builder, int index)
    {
        if (builder.Length <= 0)
            return;

        var value = builder.ToString();
        builder.Clear();
        var textToken = tokenBuilder.GetText(index - value.Length + 1, value);
        tokens.Add(textToken);
    }
}