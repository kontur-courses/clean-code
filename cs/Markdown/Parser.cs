using System.Text;
using Markdown.Extensions;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown;

public class Parser : IParser
{
    private readonly Dictionary<string?, TagType> tagDictionary;
    private List<Token> tokenList;
    private Token? previousToken;

    public Parser(Dictionary<string?, TagType> tagDictionary)
    {
        this.tagDictionary = tagDictionary;
    }

    public List<Token> Parse(string text)
    {
        var isEscapeTagActive = false;
        TokenType? previousStatus = null;
        var content = new StringBuilder();
        tokenList = new List<Token>();
        for (var index = 0; index < text.Length; index++)
        {
            var status = DetermineTokenType(text[index].ToString(), content.ToString());

            if (previousStatus == null || 
                (status == previousStatus
                  && (status == TokenType.Text
                  || (status == TokenType.Escape && content.ToString() != @"\")
                  || (status == TokenType.Tag && content.ToString().IsTagSequenceEnd(text[index].ToString(), tagDictionary)))))

            {
                content.Append(text[index]);
            }
            else
            {
                AddToken((TokenType)previousStatus, content.ToString(), text[index].ToString());
                index--;
                content.Clear();
            }

            previousStatus = status;
            if (!isEscapeTagActive && previousStatus == TokenType.Escape)
            {
                isEscapeTagActive = true;
            }
        }
        
        AddToken((TokenType)previousStatus!, content.ToString(), "");
        tokenList.Add(new Token("\n", null, TokenType.LineBreaker));
        return tokenList;
    }

    private void AddToken(TokenType status, string? content, string nextChar)
    {
        Token? token;
        if (IsEscapeTagActive(previousToken))
        {
            if (status is TokenType.Tag or TokenType.Escape)
                status = TokenType.Text;
            else
                previousToken.Type = TokenType.Text;
        }

        if (status == TokenType.Tag && content.IsTag(tagDictionary))
        {
            var tag = Tag.CreateTag(tagDictionary[content], content, previousToken, nextChar);
            token = new Token(content, tag, status);
            previousToken = token;
            tokenList.Add(token);
        }
        else
        {
            status = status == TokenType.Tag ? TokenType.Text : status;
            token = new Token(content, null, status);
            tokenList.Add(token);
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
    private static bool IsEscapeTagActive(Token previousToken)
    {
        return previousToken is { Type: TokenType.Escape };
    }
}