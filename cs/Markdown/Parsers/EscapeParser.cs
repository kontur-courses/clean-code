using Markdown.Parsers.Interfaces;

namespace Markdown.Parsers;

public class EscapeParser : ITokenParser
{
    private readonly ParserContext context;
    private readonly HashSet<TokenType> escapableTokens =
    [
        TokenType.Underscore,
        TokenType.DoubleUnderscore,
        TokenType.WordUnderscore,
        TokenType.WordDoubleUnderscore,
        TokenType.LBracket,
        TokenType.RBracket,
        TokenType.LParenthesis,
        TokenType.RParenthesis,
        TokenType.Hash,
        TokenType.Escape,
        TokenType.Exclamation
    ];

    public EscapeParser(ParserContext context)
    {
        this.context = context;
    }

    public void Parse()
    {
        if (context.Next != null && escapableTokens.Contains(context.Next.Type))
        {
            context.Next.Type = TokenType.Text;
            context.Current.Type = TokenType.Text;
            context.Current.Value = "";
            return;
        }

        context.Current.Type = TokenType.Text;
    }
}