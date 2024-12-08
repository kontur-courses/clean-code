namespace Markdown;

public class ComplexToken(TokenType type, List<Token> childrens) : Token(type: type, childrens: childrens)
{
}
