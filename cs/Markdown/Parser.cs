using System.Text;
using Markdown.Tags;

namespace Markdown;

public class Parser
{
    private readonly Dictionary<string, TagType> tagDictionary;
    private List<Token> list;
    private string text;
    private Token previousToken;

    public Parser(Dictionary<string, TagType> tagDictionary)
    {
        this.tagDictionary =  tagDictionary;
    }

    public List<Token> Parse(string text)
    {
        this.text = text;
        list = new List<Token>();
        for (var index = 0; index < text.Length; index++)
        {
            if (IsTagStart(text[index].ToString())){
                AddToken(TokenType.Tag, ref index);
            }

            else if (text[index].ToString() == @"\"){
                AddToken(TokenType.Escape, ref index);
            }

            else{
                AddToken(TokenType.Text, ref index);
            }
        }

        list.Add(new Token("\n", null, TokenType.LineBreaker));
        return list;
    }

    private void AddToken(TokenType status, ref int index)
    {
        Token token;
        var content = new StringBuilder();
        var loopStopCondition = GetLoopStopCondition(status, content);
        while (index < text.Length && loopStopCondition(text[index]))
        {
            content.Append(text[index]);
            index++;
        }
        HandleEscapeTag(ref status);
        if (status == TokenType.Tag)
        {
            var nextChar = index == text.Length ? "" : text[index].ToString();
            var tag = Tag.CreateTag(tagDictionary[content.ToString()], content.ToString(), previousToken, nextChar);
            token = new Token(content.ToString(), tag, status);
            previousToken = token;
            list.Add(token);
        }
        else
        {
            token = new Token(content.ToString(), null, status);
            list.Add(token);
        }
        previousToken = token;
        index--;
    }

    private Func<char, bool> GetLoopStopCondition(TokenType status, StringBuilder content)
    {
        return status switch
        {
            TokenType.Text => currentChar => !IsTagStart(currentChar.ToString()) && currentChar.ToString() != @"\",
            TokenType.Tag => currentChar => IsTagSequenceEnd(content.ToString(), currentChar),
            TokenType.Escape => _ => content.ToString() == string.Empty,
            _ => _ => true
        };
    }

    private bool IsTagStart(string content)
    {
        return tagDictionary.Any(tag => tag.Key.StartsWith(content));
    }

    private bool IsTag(string content)
    {
        return tagDictionary.Any(tag => tag.Key == content);
    }

    private bool IsTagSequenceEnd(string currentContent, char currentChar)
    {
        return IsTagStart(currentContent + currentChar) || IsTag(currentContent + currentChar);
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