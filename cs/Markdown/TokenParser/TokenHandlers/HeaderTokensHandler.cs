using Markdown.Extensions;
using Markdown.Tags;
using Markdown.TokenParser.Interfaces;
using Markdown.Tokens;

namespace Markdown.TokenParser.TokenHandlers;

public class HeaderTokensHandler : ITokenHandler
{
    public List<Token> HandleLine(List<Token> line)
    {
        var result = new List<Token>();
        var position = 0;
        var addCloseTag = false;

        for (var j = 0; j < line.Count; j++)
        {
            if (IsHeader(line, j) && line[j].TagType == TagType.Header)
            {
                result.Add(new Token(TokenType.MdTag, "# ",
                    position, false, TagType.Header));

                addCloseTag = true;
            }
            else if (line[j].TagType == TagType.Header)
            {
                result.Add(new Token(TokenType.Text, "# ", position));
            }

            position += line[j].Content.Length;
        }

        if (addCloseTag)
            result.Add(CreateCloseTag(position));


        return result;
    }

    private bool IsHeader(List<Token> tokens, int index)
    {
        return index == 0 && tokens.CurrentTokenIs(TagType.Header, index);
    }

    private Token CreateCloseTag(int lastIndex)
    {
        return new Token(TokenType.MdTag, "", lastIndex + 1, true, TagType.Header);
    }
}