using Markdown.Parsers.Interfaces;

namespace Markdown.Parsers;

public class HeaderParser : ITokenParser
{
    private readonly ParserContext context;

    public HeaderParser(ParserContext context)
    {
        this.context = context;
    }

    public void Parse()
    {
        if ((context.Prev == null || context.Prev.Type == TokenType.NewLine) 
            && context.Next != null && context.Next.Value.StartsWith(" "))
        {
            context.Next.Value = "";
            return;
        }
        context.Current.Type = TokenType.Text;
    }
}