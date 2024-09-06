using System.Text;
using Markdown.Extensions;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown;

public class Parser : IParser
{
    private readonly Dictionary<string, TagType> tagDictionary;
    private List<Token> tokenList;

    public Parser(Dictionary<string, TagType> tagDictionary)
    {
        this.tagDictionary = tagDictionary;
    }

    public List<Token> Parse(string text)
    { 
        Token? token;
        Token? previousToken = null;
        var isEscapeTagActive = false;
        TokenType? previousStatus = null;
        var content = new StringBuilder();
        tokenList = new List<Token>();
        for (var index = 0; index < text.Length; index++)
        {
            var status = DetermineTokenType(text[index].ToString(), content.ToString());
            if (IsContinuationTag(previousStatus, status,text[index], content.ToString())){ 
                content.Append(text[index]);
            }
            else{
                token = CreateToken((TokenType)previousStatus, content.ToString(), text[index].ToString(), previousToken);
                previousToken = token;
                tokenList.Add(token);
                index--;
                content.Clear();
                previousToken = token;
            }

            previousStatus = status;
            if (!isEscapeTagActive && previousStatus == TokenType.Escape)
            {
                isEscapeTagActive = true;
            }
        }

        token = CreateToken((TokenType)previousStatus!, content.ToString(), "", previousToken);
        previousToken = token;
        tokenList.Add(token);
        tokenList.Add(new Token("\n", null, TokenType.LineBreaker));
        return tokenList;
    }
    
    private Token CreateToken(TokenType status, string? content, string nextChar, Token? previousToken)
    {
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
            return new Token(content, tag, status);
        }
        else
        {
            status = status == TokenType.Tag ? TokenType.Text : status;
            return new Token(content, null, status);
        }
    }

    private bool IsContinuationTag(TokenType? previousStatus, TokenType status, char currentCharacter,string content)
    {
        return previousStatus == null ||
               (status == previousStatus
                && (status == TokenType.Text
                    || (status == TokenType.Escape && content != @"\")
                    || (status == TokenType.Tag &&
                        content.IsTagSequenceEnd(currentCharacter.ToString(), tagDictionary))));
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