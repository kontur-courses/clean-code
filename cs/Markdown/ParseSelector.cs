using Markdown.Parsers;
using Markdown.Parsers.Interfaces;

namespace Markdown;

public class ParseSelector
{
    private readonly ImageParser imageParser;
    private readonly TextParser textParser;
    private readonly UnderscoresParser underscoresParser;
    private readonly HeaderParser headerParser;
    private readonly EscapeParser escapeParser;
    private readonly NewLineEofParser newLineEofParser;
    public ParseSelector(ParserContext context)
    {
        imageParser = new ImageParser(context);
        textParser = new TextParser();
        underscoresParser = new UnderscoresParser(context);
        headerParser = new HeaderParser(context);
        escapeParser = new EscapeParser(context);
        newLineEofParser = new NewLineEofParser([imageParser, underscoresParser]);
    }

    public ITokenParser GetParser(TokenType tokenType)
    {
        ITokenParser parser;
        switch (tokenType)
        {
            case TokenType.LBracket:
            case TokenType.LParenthesis:
            case TokenType.RBracket:
            case TokenType.RParenthesis:
            case TokenType.Exclamation:
            {
                parser = imageParser;
                break;
            }   
            case TokenType.Space:
            case TokenType.Underscore:
            case TokenType.DoubleUnderscore:
            case TokenType.WordUnderscore:
            case TokenType.WordDoubleUnderscore:
            {
                parser = underscoresParser;
                break;
            }
            case TokenType.Hash:
            {
                parser = headerParser;
                break;
            }
            case TokenType.Escape:
            {
                parser = escapeParser;
                break;
            }
            case TokenType.NewLine:
            case TokenType.Eof:
            {
                parser = newLineEofParser;
                break;
            }
            default:
            {
                parser = textParser;
                break;
            }
        }

        return parser;
    }
}