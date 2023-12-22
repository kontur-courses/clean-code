using System.Text;
using Markdown.Extensions;
using Markdown.Tags;

namespace Markdown;

public class Parser
{
    private readonly Dictionary<string, TagType> tagDictionary;
    private List<Token> list;
    private Token? previousToken;

    public Parser(Dictionary<string, TagType> tagDictionary)
    {
        this.tagDictionary = tagDictionary;
    }

    public List<Token> Parse(string text)
    {
        var previousStatus = TokenType.Undefined;
        var content = new StringBuilder();
        list = new List<Token>();
        for (var index = 0; index < text.Length; index++)
        {
            var status = DetermineTokenType(text[index].ToString(), content.ToString());

            if (previousStatus == TokenType.Undefined || 
                (status == previousStatus
                  && (status == TokenType.Text
                  || (status == TokenType.Escape && content.ToString() != @"\")
                  || (status == TokenType.Tag && content.ToString().IsTagSequenceEnd(text[index].ToString(), tagDictionary)))))

            {
                content.Append(text[index]);
            }
            else
            {
                HandleEscapeTag(ref previousStatus);
                AddToken(previousStatus, content.ToString(), text[index].ToString());
                index--;
                content.Clear();
            }

            previousStatus = status;
        }

        HandleEscapeTag(ref previousStatus);
        AddToken(previousStatus, content.ToString(), "");
        list.Add(new Token("\n", null, TokenType.LineBreaker));
        return list;
    }

    private void AddToken(TokenType status, string content, string nextChar)
    {
        Token? token;
        if (status == TokenType.Tag && content.IsTag(tagDictionary))
        {
            var tag = Tag.CreateTag(tagDictionary[content], content, previousToken, nextChar);
            token = new Token(content, tag, status);
            previousToken = token;
            list.Add(token);
        }
        else
        {
            status = status == TokenType.Tag ? TokenType.Text : status;
            token = new Token(content, null, status);
            list.Add(token);
        }

        previousToken = token;
    }

    private TokenType DetermineTokenType(string currentChar, string content)
    {
        return currentChar switch
        {
            _ when currentChar.IsTagStart(tagDictionary) || content.IsTagSequenceEnd(currentChar, tagDictionary) => TokenType.Tag,
            @"\" => TokenType.Escape,
            _ => TokenType.Text
        };
    }
    private bool IsEscapeTag()
    {
        return list.Count > 0 && list.Last().Type == TokenType.Escape;
    }

    private void HandleEscapeTag(ref TokenType status)
    {
        if (!IsEscapeTag()) return;
        if (status is TokenType.Tag or TokenType.Escape)
            status = TokenType.Text;
        else
            list.Last().Type = TokenType.Text;
    }
}