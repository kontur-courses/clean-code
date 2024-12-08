using Markdown.Token;

namespace Markdown.Parser.TokenHandler.Handlers;

public class LinkHandler : BaseTokenHandler
{
    public LinkHandler() : base(new Delimiter("[", "]", TokenType.Link))
    {
    }

    public override bool TryHandle(ParsingContext context, out Token.Token token, out int skip)
    {
        token = null;
        skip = 0;
        var position = context.Position;
        var text = context.Text;

        if (text[position] != '[')
            return false;

        var closingBracketIndex = text.IndexOf(']', position);
        if (closingBracketIndex == -1 || closingBracketIndex + 1 >= text.Length || text[closingBracketIndex + 1] != '(')
            return false;

        var closingParentIndex = text.IndexOf(')', closingBracketIndex + 1);
        if (closingParentIndex == -1)
            return false;

        var linkText = text.Substring(position + 1, closingBracketIndex - position - 1);
        var url = text.Substring(closingBracketIndex + 2, closingParentIndex - closingBracketIndex - 2);

        token = new Token.Token(linkText, TokenType.Link, TagState.Open, position, url: url);
        skip = closingParentIndex - position + 1;

        return true;
    }
}