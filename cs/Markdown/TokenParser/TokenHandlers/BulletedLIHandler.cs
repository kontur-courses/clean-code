using Markdown.Extensions;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenParser.TokenHandlers;

public class BulletedLIHandler
{
    public List<Token> HandleLine(List<Token> line)
    {
        var result = new List<Token>();
        var position = 0;
        var addCloseTag = false;

        for (var j = 0; j < line.Count; j++)
        {
            if (IsBulletedListItem(line, j) && line[j].TagType == TagType.BulletedListItem)
            {
                result.Add(new Token(TokenType.MdTag, "* ",
                    position, false, TagType.BulletedListItem));

                addCloseTag = true;
            }
            else if (line[j].TagType == TagType.BulletedListItem)
            {
                result.Add(new Token(TokenType.Text, "* ", position));
            }

            position += line[j].Content.Length;
        }

        if (addCloseTag)
            result.Add(CreateCloseTag(position));


        return result;
    }

    private bool IsBulletedListItem(List<Token> tokens, int index)
    {
        return index == 0 && tokens.CurrentTokenIs(TagType.BulletedListItem, index);
    }

    private Token CreateCloseTag(int lastIndex)
    {
        return new Token(TokenType.MdTag, "", lastIndex + 1, true, TagType.BulletedListItem);
    }
}